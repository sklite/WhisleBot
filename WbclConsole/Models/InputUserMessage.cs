﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhisleBotConsole.Models;

namespace BotServer.Models
{
    public class InputUserMessage : IMessage
    {
        public int UserId { get; set; }
        public long ChatId { get; set; }
        public string Text { get; set; }
    }
}
