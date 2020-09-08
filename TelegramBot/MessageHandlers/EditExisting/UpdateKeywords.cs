using BotServer.Vk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using VkNet.Enums.SafetyEnums;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class UpdateKeywords : BaseTgMessageHandler
    {


        public UpdateKeywords(UsersContext db)
            : base(db)
        {
        }

        OutputUserMessage FailWithText(Message inputMessage, User user, string text)
        {
            user.State = ChatState.Standrard;
            _db.SaveChanges();
            return GetDefaultResponse(inputMessage.Chat.Id, text);
        }

        public override OutputUserMessage GetResponseTo(Message inputMessage, User user)
        {
            if (!inputMessage.Text.Contains("(id: "))
                return FailWithText(inputMessage, user, "Не удалось получить id группы");

            var idStr = inputMessage.Text.Split("(id: ", StringSplitOptions.RemoveEmptyEntries).Last();
            idStr = idStr.Split(")", StringSplitOptions.RemoveEmptyEntries).First();
            idStr = new string(idStr.Where(char.IsDigit).ToArray());
            if (!long.TryParse(idStr, out long groupId))
                return FailWithText(inputMessage, user, "Не удалось получить id группы");

            user.CurrentGroupId = groupId;
            user.State = ChatState.NewWordToGroupAdd;

            var currentText = _db.Preferences.Where(pref => pref.User.Id == user.Id && pref.GroupId == groupId).FirstOrDefault();

            return new OutputUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = $"Текущие ключевые слова группы: _{currentText.Keyword}_. Укажите новые ключевые слова у группы:",
                ReplyMarkup = new ReplyKeyboardRemove()
            };
        }
    }
}
