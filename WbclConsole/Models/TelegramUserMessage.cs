using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhisleBotConsole.Models
{
    class TelegramUserMessage : IMessage
    {
        public long ChatId { get; set; }
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup{ get; set; }
        public InputOnlineFile File { get; set; }
    }
}
