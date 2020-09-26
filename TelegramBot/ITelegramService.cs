using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhisleBotConsole.Models;

namespace BotServer.TelegramBot
{
    interface ITelegramService
    {
        bool Start();
        bool Stop();
       // Task SendMessages(List<TelegramUserMessage> messages);
    }
}
