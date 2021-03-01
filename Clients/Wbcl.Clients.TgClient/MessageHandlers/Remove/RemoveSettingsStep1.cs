using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Wbcl.Clients.TelegramClient.MessageHandlers;
using Wbcl.Clients.TelegramClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.DAL.Context;
using WhisleBotConsole.TelegramBot.MarkupUtils;
using User = Wbcl.Core.Models.Database.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class RemoveSettingsStep1 : BaseTgMessageHandler
    {

        public RemoveSettingsStep1(IUsersContext db)
            : base(db)
        {
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, User user)
        {
            var currentSubscriptions = _db.Preferences.Where(pref => pref.User.Id == user.Id);
            var keyboard = MessageMarkupUtilities.GetReplyKeyboardForGroups(currentSubscriptions);
            keyboard.Add(new List<KeyboardButton> { new KeyboardButton(TgBotText.Cancel) });

            user.State = ChatState.RemoveSettingsStep1;
            _db.SaveChanges();

            var replyMarkup = new ReplyKeyboardMarkup(keyboard, resizeKeyboard: true, oneTimeKeyboard: true);

            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = TgBotText.RemoveSubscriptionsLink,
                ReplyMarkup = replyMarkup
            };
        }

        public override ChatState UsedChatState => ChatState.NotUsed;
        public override string UsedUserInput => TgBotText.RemoveSubscriptions;
    }
}
