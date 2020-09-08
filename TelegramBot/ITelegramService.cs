using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotServer.TelegramBot
{
    interface ITelegramService
    {
        bool Start();
        bool Stop();
    }
}
