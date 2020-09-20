﻿using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MarkupUtils;
using WhisleBotConsole.Vk;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class RemoveSettingsStep1 : BaseTgMessageHandler
    {
        private readonly IVkGroupsSearcher _vk;

        public RemoveSettingsStep1(UsersContext db, IVkGroupsSearcher vk)
            : base(db)
        {
            _vk = vk;
        }

        public override OutputUserMessage GetResponseTo(Message inputMessage, User user)
        {
            var currentSubscriptions = _db.Preferences.Where(pref => pref.User.Id == user.Id);
            var keyboard = MessageMarkupUtilities.GetReplyKeyboardForGroups(currentSubscriptions);
            keyboard.Add(new List<KeyboardButton> { new KeyboardButton(TgBotText.Cancel) });

            user.State = ChatState.RemoveSettingsStep1;
            _db.SaveChanges();

            var replyMarkup = new ReplyKeyboardMarkup(keyboard, resizeKeyboard: true, oneTimeKeyboard: true);

            return new OutputUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = TgBotText.RemoveSubscriptionsLink,
                ReplyMarkup = replyMarkup
            };
        }
    }
}
