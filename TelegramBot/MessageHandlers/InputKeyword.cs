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
    class InputKeyword : BaseTgMessageHandler
    {
        public InputKeyword(UsersContext _db)
            : base(_db)
        {

        }
        public override OutputUserMessage GetResponseTo(Message inputMessage, User user)
        {
            if (string.IsNullOrEmpty(inputMessage.Text))
            {
                return GetDefaultResponse(inputMessage.Chat.Id, "Пустой текст не является ключевым словом");
            }

            if (user.CurrentGroupId == null)
            {
                user.CurrentGroupId = null;
                user.CurrentGroupName = null;
                user.State = ChatState.Standrard;
                _db.SaveChanges();
                return GetDefaultResponse(inputMessage.Chat.Id, @"Что-то пошло не так\. Попробуйте ещё раз");
            }

            var userPrefs = _db.Preferences.Where(pref => pref.User.Id == user.Id && pref.GroupId == user.CurrentGroupId).FirstOrDefault();

            if (userPrefs == null)
            {
                userPrefs = new UserPreference()
                {
                    User = user,
                    GroupId = user.CurrentGroupId.Value,
                    GroupName = user.CurrentGroupName,
                    Keyword = inputMessage.Text
                };
                _db.Preferences.Add(userPrefs);
            }
            else
            {
                userPrefs.Keyword = inputMessage.Text;
            }

            user.CurrentGroupId = null;
            user.CurrentGroupName = null;
            user.State = ChatState.Standrard;
            _db.SaveChanges();

            return new OutputUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = @$"Отлично. Слова записаны. Когда в группе *{userPrefs.GroupName}* (id:_{userPrefs.GroupId}_) появятся новые посты со следюующими словами: _{inputMessage.Text}_ Вы получите уведомление сюда",
                ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
            };
        }
    }
}
