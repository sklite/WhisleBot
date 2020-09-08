using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MarkupUtils;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    abstract class BaseTgMessageHandler
    {
        protected readonly UsersContext _db;

        public BaseTgMessageHandler(UsersContext db)
        {
            _db = db;
        }

        public abstract OutputUserMessage GetResponseTo(Message inputMessage, User user);

        public static OutputUserMessage GetDefaultResponse(long chatId, string additionalText = "")
        {
            return new OutputUserMessage()
            {
                ChatId = chatId,
                Text = $"{additionalText}\nВыбери что тебе нужно сделать:",
                ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
            };
        }
    }
}
