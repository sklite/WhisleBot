using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WhisleBotConsole.Models;

namespace WhisleBotConsole.TelegramBot
{
    interface ITelegramMessageRouter
    {
        Task ProcessMessageAsync(Message inputMessage);
    }
}
