using BotServer.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot;

namespace BotServer.TelegramBot
{
    class TelegramBotService : ITelegramService
    {
        readonly Logger _logger;
        readonly ITelegramMessageRouter _messageRouter;
        readonly ITelegramBotClient _botClient;
        public TelegramBotService(ITelegramMessageRouter messageRouter, ITelegramBotClient botClient)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _messageRouter = messageRouter;
            _botClient = botClient;
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;

        }
        public bool Start()
        {
            _botClient.StartReceiving();
            return true;
        }

        public bool Stop()
        {
            _botClient.StopReceiving();
            return true;
        }

        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.Message.Text != null)
                {
                    _messageRouter.ProcessMessageAsync(e.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception occured in TelegramBotService {ex.ToString()}");
            }
        }
    }
}
