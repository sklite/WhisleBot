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
using Telegram.Bot.Types.ReplyMarkups;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.TelegramBot;

namespace BotServer.TelegramBot
{
    class TelegramBotService : ITelegramService
    {
        readonly ITelegramMessageRouter _messageRouter;
        readonly UsersContext _context;
        readonly Settings _settings;
        readonly ITelegramBotClient botClient;
        public TelegramBotService(ITelegramMessageRouter messageRouter, UsersContext context, IOptions<Settings> settings)
        {
            _messageRouter = messageRouter;
            _context = context;
            _settings = settings.Value;
            botClient = new TelegramBotClient(_settings.Telegram.AccessT);
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;

        }

        public bool Start()
        {
            botClient.StartReceiving();
            return true;
        }

        public bool Stop()
        {
            botClient.StopReceiving();
            return true;
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                var client = new HttpClient();

                var userMessage = new InputUserMessage
                {
                    ChatId = e.Message.Chat.Id,
                    Text = e.Message.Text
                };

                //var reuslt = client.PostAsync("https://localhost:44304/api/botapp/send",
                //    new StringContent(JsonConvert.SerializeObject(userMessage), Encoding.UTF8, "application/json")).Result;



                var outMessage = _messageRouter.ProcessMessage(e.Message);

                await botClient.SendTextMessageAsync(
                    outMessage.ChatId,
                    outMessage.Text,
                    Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    replyMarkup: outMessage.ReplyMarkup);
            }
        }
    }
}
