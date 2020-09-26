using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using WhisleBotConsole.Vk;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class InputGroup : BaseTgMessageHandler
    {
        private readonly IVkGroupsSearcher _vk;

        public InputGroup(UsersContext db, IVkGroupsSearcher vk)
            : base(db)
        {
            _vk = vk;
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, User user)
        {
            if (string.IsNullOrEmpty(inputMessage.Text))            
                return FailWithText(inputMessage, user, "Введено пустое слово");

            var inputText = inputMessage.Text;

            if (!Uri.TryCreate(inputText, UriKind.Absolute, out Uri uriResult))            
                return FailWithText(inputMessage, user, "Введён некорректный URL");

            
            var getGroupIdResult = _vk.GetGroupIdByLink(uriResult);
            if (!getGroupIdResult.Success)
                return FailWithText(inputMessage, user, "Не удалось получть id группы");

            user.CurrentGroupId = getGroupIdResult.GroupId;
            user.CurrentGroupName = getGroupIdResult.GroupName;
            user.State = ChatState.NewWordToGroupAdd;
            _db.SaveChanges();

            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = @"Введите слова или фразы через запятую, какие следует искать в этой группе. Например _однушка, однушку, перекопка, торты, аренда_",
                ReplyMarkup = new ReplyKeyboardRemove()
            };
        }
    }
}
