using NLog;
using System.Threading.Tasks;
using Telegram.Bot;
using WhisleBotConsole.Models;

namespace WhisleBotConsole.BotContorller
{
    class TelegramMessageSender : IMessageSender
    {
        readonly ITelegramBotClient _botClient;
        private readonly Logger _logger;

        public TelegramMessageSender(ITelegramBotClient botCLient)
        {
            _botClient = botCLient;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task SendMessageToUser(IMessage message)
        {
            if (message == null)
                return;

            var tgMessage = message as TelegramUserMessage;

            _logger.Info($"Messaging chat {message.ChatId} with text \"{message.Text}\"");

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
        }
    }
}
