using System;
using System.Collections.Generic;
using System.Text;

namespace WhisleBotConsole.Models
{
    interface IMessage
    {
        long ChatId { get; set; }
        string Text { get; set; }
    }
}
