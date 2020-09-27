using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class Step1AddNewAlarms : BaseTgMessageHandler
    {
        public Step1AddNewAlarms(UsersContext _db)
            :base(_db)
        {

        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, User user)
        {
            user.State = ChatState.NewGroupToAdd;
            _db.SaveChanges();


            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = TgBotText.ReplyInputIdOrLink,
                ReplyMarkup = new ReplyKeyboardRemove()
            };
        }
    }
}
