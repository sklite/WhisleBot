using System;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Args;
using Wbcl.Core.Models.Services;

namespace Wbcl.Clients.TgClient
{
    public class TelegramBotService : IClientService
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
              $"Telegram bot client instantiated. I am user {me.Id} and my name is {me.FirstName}."
            );

            botClient.OnMessage += Bot_OnMessage;

        }
        public void Start()
        {
            _botClient.StartReceiving();
        }

        public void Stop()
        {
            _botClient.StopReceiving();
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
