using System;
using System.Linq;
using Telegram.Bot.Types;
using Wbcl.Clients.TelegramClient.MessageHandlers;
using Wbcl.Clients.TelegramClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.DAL.Context;
using WhisleBotConsole.TelegramBot.MarkupUtils;
using User = Wbcl.Core.Models.Database.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class RemoveSettingsStep2 : BaseTgMessageHandler
    {

        public RemoveSettingsStep2(IUsersContext db)
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

            var groupsToRemove = _db.Preferences.Where(pref => pref.User.Id == user.Id && pref.TargetId == groupId);
            if (groupsToRemove == null || !groupsToRemove.Any())
            {
                return FailWithText(inputMessage.Chat.Id, user, $"Группа с указанным id:{groupId} не найдена в подписках");
            }
            var groupName = groupsToRemove.First().TargetName;
            _db.Preferences.RemoveRange(groupsToRemove);
            user.State = ChatState.Standrard;
            _db.SaveChanges();
            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = $"Подписки на ключевые слова группы *{groupName}* успешно удалены",
                ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
            };
        }

        public override ChatState UsedChatState => ChatState.RemoveSettingsStep1;
        public override string UsedUserInput => string.Empty;
    }
}
