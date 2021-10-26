using System;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Wbcl.Clients.TgClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.DAL.Context;
using User = Wbcl.Core.Models.Database.User;

namespace Wbcl.Clients.TgClient.MessageHandlers.EditExisting
{
    class UpdateKeywords : BaseTgMessageHandler
    {
        public UpdateKeywords(IUsersContext db)
            : base(db)
        {
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, User user)
        {
            if (inputMessage.Text == TgBotText.Cancel)
                return FailWithText(inputMessage.Chat.Id, user, "Ну передумал и передумал.");

            if (!inputMessage.Text.Contains("(id: "))
                return FailWithText(inputMessage.Chat.Id, user, "Не удалось получить id группы");

            var idStr = inputMessage.Text.Split("(id: ", StringSplitOptions.RemoveEmptyEntries).Last();
            idStr = idStr.Split(")", StringSplitOptions.RemoveEmptyEntries).First();
            idStr = new string(idStr.Where(char.IsDigit).ToArray());
            if (!long.TryParse(idStr, out long groupId))
                return FailWithText(inputMessage.Chat.Id, user, "Не удалось получить id группы");

            user.CurrentTargetId = groupId;
            user.State = ChatState.NewWordToGroupAdd;
            _db.SaveChanges();

            var currentText = _db.Preferences.Where(pref => pref.User.Id == user.Id && pref.TargetId == groupId).FirstOrDefault();

            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = $"Текущие ключевые слова группы: _{currentText.Keyword}_. Укажите новые ключевые слова у группы:",
                ReplyMarkup = new ReplyKeyboardRemove()
            };
        }

        public override ChatState UsedChatState => ChatState.EditExistingGroup;

        public override string UsedUserInput => string.Empty;
    }
}
