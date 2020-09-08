using System;
using System.Collections.Generic;
using System.Text;

namespace WhisleBotConsole.Models
{
    interface ITelegramMessage
    {
        int UserId { get; set; }
        long ChatId { get; set; }
        string Text { get; set; }
    }
}
