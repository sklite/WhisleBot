using BotServer.Vk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VkNet.Enums.SafetyEnums;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MarkupUtils;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class EditExistingSettings : BaseTgMessageHandler
    {
        private readonly VkGroupsSearcher _vk;

        public EditExistingSettings(UsersContext db, VkGroupsSearcher vk)
            : base(db)
        {
            _vk = vk;
        }

        public override OutputUserMessage GetResponseTo(Message inputMessage, User user)
        {
            var currentSubscriptions = _db.Preferences.Where(pref => pref.User.Id == user.Id);
            var keyboard = MessageMarkupUtilities.GetReplyKeyboardForGroups(currentSubscriptions);

            user.State = ChatState.EditExistingGroup;
            _db.SaveChanges();

            var replyMarkup = new ReplyKeyboardMarkup(keyboard, resizeKeyboard: true, oneTimeKeyboard: true);

            return new OutputUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = TgBotText.EditCurrentSubscriptionsLink,
                ReplyMarkup = replyMarkup
            };
        }
    }
}
