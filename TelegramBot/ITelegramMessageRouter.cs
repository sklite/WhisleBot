using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using WhisleBotConsole.Models;

namespace WhisleBotConsole.TelegramBot
{
    interface ITelegramMessageRouter
    {
        OutputUserMessage ProcessMessage(Message inputMessage);
    }
}
