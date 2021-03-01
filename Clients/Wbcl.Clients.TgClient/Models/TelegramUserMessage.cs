using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Wbcl.Core.Models.Notifications;

namespace Wbcl.Clients.TelegramClient.Models
{
    class TelegramUserMessage : IMessage
    {
        public long ChatId { get; set; }
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup { get; set; }
        public InputOnlineFile File { get; set; }
    }
}
