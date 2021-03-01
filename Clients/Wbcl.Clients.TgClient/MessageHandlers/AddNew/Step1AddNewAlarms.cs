using Microsoft.Extensions.Options;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Wbcl.Clients.TelegramClient.MessageHandlers;
using Wbcl.Clients.TelegramClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.Core.Models.Settings;
using Wbcl.DAL.Context;
using User = Wbcl.Core.Models.Database.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class Step1AddNewAlarms : BaseTgMessageHandler
    {
        private readonly Settings _settings;

        public Step1AddNewAlarms(IUsersContext _db, Settings settings)
            :base(_db)
        {
            _settings = settings;
        }
        public override TelegramUserMessage GetResponseTo(Message inputMessage, User user)
        {
            var prefs = _db.Preferences.Where(pref => pref.User == user);
            if (user.SubscriptionStatus == UserType.StandardUser && prefs.Count() >= _settings.Vkontakte.BaseSubscriptionsLimit)
            {
                return FailWithText(inputMessage.Chat.Id, user, $"На данный момент лимит подписок ограничивается {_settings.Vkontakte.BaseSubscriptionsLimit}" +
                    $" группами. Для того, чтобы подписаться на новые уведомления групп, отпишитесь от старых.");
            }

            if (user.SubscriptionStatus == UserType.ExtendedUser && prefs.Count() >= _settings.Vkontakte.ExtendedSubscriptionsLimit)
            {
                return FailWithText(inputMessage.Chat.Id, user, $"На данный момент ваш лимит подписок ограничивается {_settings.Vkontakte.ExtendedSubscriptionsLimit}" +
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

        public override ChatState UsedChatState => ChatState.NotUsed;
        public override string UsedUserInput => TgBotText.AddNewSettings;
    }
}
