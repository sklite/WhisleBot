using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhisleBotConsole.Models;

namespace WhisleBotConsole.BotContorller
{
    interface IMessageSender
    {
        Task SendMessageToUser(IMessage message);
    }
}
