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

        public abstract TelegramUserMessage GetResponseTo(Message inputMessage, User user);

        protected TelegramUserMessage FailWithText(long chatId, User user, string text)
        {
            user.State = ChatState.Standrard;
            _db.SaveChanges();
            return GetDefaultResponse(chatId, text);
        }

        public static TelegramUserMessage GetDefaultResponse(long chatId, string additionalText = "")
        {
            return new TelegramUserMessage()
            {
                ChatId = chatId,
                Text = $"{additionalText}\nВыбери что тебе нужно сделать:",
                ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
            };
        }
    }
}
