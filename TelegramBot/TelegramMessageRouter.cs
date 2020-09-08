using BotServer.Vk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MessageHandlers;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot
{
    class TelegramMessageRouter : ITelegramMessageRouter
    {
        Dictionary<ChatState, BaseTgMessageHandler> _messageHandlers;
        Dictionary<string, BaseTgMessageHandler> _commandHandlers;
        private readonly UsersContext _db;

        public TelegramMessageRouter(UsersContext db, VkGroupsSearcher vk)
        {
            _db = db;
            _messageHandlers = new Dictionary<ChatState, BaseTgMessageHandler>
            {
                { ChatState.Standrard, new AddNewAlarms(_db) },
                { ChatState.NewGroupToAdd, new InputGroup(_db, vk) },
                { ChatState.NewWordToGroupAdd, new InputKeyword(_db) },
                { ChatState.EditExistingGroup, new UpdateKeywords(_db) }
            };

            _commandHandlers = new Dictionary<string, BaseTgMessageHandler>
            {
                { TgBotText.AddNewSettings, new AddNewAlarms(_db) },
                { TgBotText.EditExistingSettings, new EditExistingSettings(_db, vk) }
            };
        }

        protected User GetOrCreateUser(long chatId)
        {
            var user = _db.Users.Where(user => user.ChatId == chatId).FirstOrDefault();
            if (user == null)
            {
                user = new DB.User() { ChatId = chatId };
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            return user;
        }


        public OutputUserMessage ProcessMessage(Message inputMessage)
        {
            if (inputMessage == null || string.IsNullOrEmpty(inputMessage.Text))
                return BaseTgMessageHandler.GetDefaultResponse(inputMessage.Chat.Id);

            var user = GetOrCreateUser(inputMessage.Chat.Id);

            if (user.State == ChatState.NewGroupToAdd)
                return _messageHandlers[ChatState.NewGroupToAdd].GetResponseTo(inputMessage, user);

            if (user.State == ChatState.NewWordToGroupAdd)
                return _messageHandlers[ChatState.NewWordToGroupAdd].GetResponseTo(inputMessage, user);

            if (user.State == ChatState.EditExistingGroup)
                return _messageHandlers[ChatState.EditExistingGroup].GetResponseTo(inputMessage, user);

            if (_commandHandlers.ContainsKey(inputMessage.Text))
                return _commandHandlers[inputMessage.Text].GetResponseTo(inputMessage, user);

            
            return BaseTgMessageHandler.GetDefaultResponse(inputMessage.Chat.Id);
        }

    }
}
