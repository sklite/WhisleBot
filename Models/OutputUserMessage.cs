using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhisleBotConsole.Models
{
    class OutputUserMessage : ITelegramMessage
    {
        public int UserId { get; set; }
        public long ChatId { get; set; }
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup{ get; set; }
    }
}
