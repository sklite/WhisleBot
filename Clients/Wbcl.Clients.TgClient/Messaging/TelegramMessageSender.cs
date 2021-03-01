using NLog;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Wbcl.Clients.TelegramClient.Models;
using Wbcl.Core.Models.Notifications;
using Wbcl.Core.Models.Services;
using Wbcl.DAL.Context;

namespace Wbcl.Clients.TelegramClient
{
    public class TelegramMessageSender : IMessageSender
    {
        readonly ITelegramBotClient _botClient;
        private readonly IHistoryLogger _history;
        private readonly Logger _logger;

        public TelegramMessageSender(ITelegramBotClient botCLient, IHistoryLogger history)
        {
            _botClient = botCLient;
            _history = history;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task SendMessageToUser(IMessage message)
        {
            if (message == null)
                return;
            _logger.Info($"Messaging chat {message.ChatId} with text \"{message.Text}\"");

            _history.LogHistory(message.ChatId, DateTime.Now, true, message.Text);

            var tgMessage = message as TelegramUserMessage;
            if (tgMessage != null)
            {
                if (tgMessage.File != null)
                {
                    await _botClient.SendDocumentAsync(tgMessage.ChatId,
                        tgMessage.File,
                        tgMessage.Text,
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyMarkup: tgMessage.ReplyMarkup);
                }
                else
                {
                    await _botClient.SendTextMessageAsync(
                        tgMessage.ChatId,
                        tgMessage.Text,
                        Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        replyMarkup: tgMessage.ReplyMarkup);
                }
                return;
            }

            var notificationMessage = message as NotificationMesasge;
            if (notificationMessage != null)
            {
                await _botClient.SendTextMessageAsync(
                        notificationMessage.ChatId,
                        notificationMessage.Text,
                        Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
        }
    }
}
