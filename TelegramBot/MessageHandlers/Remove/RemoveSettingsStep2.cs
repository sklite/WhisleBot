using System;
using System.Linq;
using Telegram.Bot.Types;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MarkupUtils;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class RemoveSettingsStep2 : BaseTgMessageHandler
    {

        public RemoveSettingsStep2(UsersContext db)
            : base(db)
        {
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, User user)
        {
            if (inputMessage.Text == TgBotText.Cancel)
                return FailWithText(inputMessage, user, "Ну передумал и передумал.");
            if (!inputMessage.Text.Contains("(id: "))
                return FailWithText(inputMessage, user, "Не удалось получить id группы");

            var idStr = inputMessage.Text.Split("(id: ", StringSplitOptions.RemoveEmptyEntries).Last();
            idStr = idStr.Split(")", StringSplitOptions.RemoveEmptyEntries).First();
            idStr = new string(idStr.Where(char.IsDigit).ToArray());
            if (!long.TryParse(idStr, out long groupId))
                return FailWithText(inputMessage, user, "Не удалось получить id группы");

            var groupsToRemove = _db.Preferences.Where(pref => pref.User.Id == user.Id && pref.GroupId == groupId);
            if (groupsToRemove == null || !groupsToRemove.Any())
            {
                return FailWithText(inputMessage, user, $"Группа с указанным id:{groupId} не найдена в подписках");
            }
            _db.Preferences.RemoveRange(groupsToRemove);
            user.State = ChatState.Standrard;
            _db.SaveChanges();
            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = "Подписки на ключевые слова групп успешно удалены",
                ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
            };
        }
    }
}
