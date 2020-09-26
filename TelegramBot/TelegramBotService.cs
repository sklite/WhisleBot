using BotServer.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        readonly ITelegramMessageRouter _messageRouter;
        readonly UsersContext _context;
        readonly Settings _settings;
        readonly ITelegramBotClient _botClient;
        public TelegramBotService(ITelegramMessageRouter messageRouter, ITelegramBotClient botClient, UsersContext context, IOptions<Settings> settings)
        {
            _messageRouter = messageRouter;
            _context = context;
            _settings = settings.Value;
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
            if (e.Message.Text != null)
            {
                _messageRouter.ProcessMessageAsync(e.Message);
            }
        }
        //public async Task SendMessages(List<TelegramUserMessage> messages)
        //{
        //    if (messages == null || !messages.Any())
        //        return;

        //    foreach (var message in messages)
        //    {
        //        await botClient.SendTextMessageAsync(
        //            message.ChatId,
        //            message.Text,
        //            Telegram.Bot.Types.Enums.ParseMode.Markdown,
        //            replyMarkup: message.ReplyMarkup);
        //    }
        //}
    }
}
