using System;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Wbcl.Clients.TelegramClient.MessageHandlers;
using Wbcl.Clients.TelegramClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.Core.Utils;
using Wbcl.DAL.Context;
using User = Wbcl.Core.Models.Database.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class Step2InputGroup : BaseTgMessageHandler
    {
        private readonly IVkUtils _vk;
        private Dictionary<PreferenceType, string> _resultText;


        public Step2InputGroup(IUsersContext db, IVkUtils vk)
            : base(db)
        {
            _vk = vk;
            _resultText = new Dictionary<PreferenceType, string> {
                { PreferenceType.VkGroup, "в этой группе" },
                { PreferenceType.VkUser, "на стене пользователя" }
            };
        }
        public override TelegramUserMessage GetResponseTo(Message inputMessage, User user)
        {
            if (string.IsNullOrEmpty(inputMessage.Text))            
                return FailWithText(inputMessage.Chat.Id, user, "Введено пустое слово");

            var inputText = inputMessage.Text;

            if (!Uri.TryCreate(inputText, UriKind.Absolute, out Uri uriResult))            
                return FailWithText(inputMessage.Chat.Id, user, "Введён некорректный URL");

            
            var getGroupIdResult = _vk.GetObjIdIdByLink(uriResult);
            if (!getGroupIdResult.Success)
                return FailWithText(inputMessage.Chat.Id, user, "Не удалось получть id группы");

            user.CurrentTargetId = getGroupIdResult.Id;
            user.CurrentTargetName = getGroupIdResult.Name;
            user.CurrentTargetType = getGroupIdResult.LinkType;
            user.State = ChatState.NewWordToGroupAdd;
            _db.SaveChanges();

            var messageText = $"Введите слова или фразы через запятую, какие следует искать {_resultText[getGroupIdResult.LinkType]}. Например _однушка, перекопка, торты, аквариум, аренда_.";

            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = messageText,
                ReplyMarkup = new ReplyKeyboardRemove()
            };
        }

        public override ChatState UsedChatState => ChatState.NewGroupToAdd;
        public override string UsedUserInput => string.Empty;
    }
}
