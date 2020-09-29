using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class Step1AddNewAlarms : BaseTgMessageHandler
    {
        private readonly Settings _settings;

        public Step1AddNewAlarms(UsersContext _db, IOptions<Settings> settings)
            :base(_db)
        {
            _settings = settings.Value;
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, User user)
        {
            var prefs = _db.Preferences.Where(pref => pref.User == user);
            if (prefs.Count() >= _settings.Vkontakte.BaseSubscriptionsLimit)
            {
                return FailWithText(inputMessage.Chat.Id, user, $"На данный момент лимит подписок ограничивается {_settings.Vkontakte.BaseSubscriptionsLimit}" +
                    $" группами. Для того, чтобы подписаться на новые уведомления групп, отпишитесь от старых.");
            }

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
