using BotServer.Vk;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class InputGroup : BaseTgMessageHandler
    {
        private readonly VkGroupsSearcher _vk;

        public InputGroup(UsersContext db, VkGroupsSearcher vk)
            : base(db)
        {
            _vk = vk;
        }

        OutputUserMessage FailWithText(Message inputMessage, User user, string text)
        {
            user.State = ChatState.Standrard;
            _db.SaveChanges();
            return GetDefaultResponse(inputMessage.Chat.Id, text);
        }

        public override OutputUserMessage GetResponseTo(Message inputMessage, User user)
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

            return new OutputUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = @"Введите слова или фразы через запятую, какие следует искать в этой группе. Например _однушка, однушку, перекопка, торты, аренда_",
                ReplyMarkup = new ReplyKeyboardRemove()
            };
        }
    }
}
