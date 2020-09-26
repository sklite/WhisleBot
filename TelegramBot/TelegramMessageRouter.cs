using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WhisleBotConsole.BotContorller;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MessageHandlers;
using WhisleBotConsole.Vk;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot
{
    class TelegramMessageRouter : ITelegramMessageRouter
    {
        Dictionary<ChatState, BaseTgMessageHandler> _messageHandlers;
        Dictionary<string, BaseTgMessageHandler> _commandHandlers;
        private readonly UsersContext _db;
        private readonly IMessageSender _messageSender;
        private readonly Logger _logger;

        public TelegramMessageRouter(UsersContext db, IVkGroupsSearcher vk, IMessageSender messageSender)
        {
            _db = db;
            _messageSender = messageSender;
            _logger = LogManager.GetCurrentClassLogger();
            _messageHandlers = new Dictionary<ChatState, BaseTgMessageHandler>
            {
                { ChatState.NewGroupToAdd, new InputGroup(_db, vk) },
                { ChatState.NewWordToGroupAdd, new InputKeyword(_db) },
                { ChatState.EditExistingGroup, new UpdateKeywords(_db) },
                { ChatState.RemoveSettingsStep1, new RemoveSettingsStep2(_db) }
            };

            _commandHandlers = new Dictionary<string, BaseTgMessageHandler>
            {
                { TgBotText.AddNewSettings, new AddNewAlarms(_db) },
                { TgBotText.EditExistingSettings, new EditExistingSettings(_db, vk) },
                { TgBotText.RemoveSubscriptions, new RemoveSettingsStep1(_db, vk) }
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


        public async Task ProcessMessageAsync(Message inputMessage)
        {
            IMessage resultMessage = null;

            if (inputMessage == null || string.IsNullOrEmpty(inputMessage.Text))
                resultMessage = BaseTgMessageHandler.GetDefaultResponse(inputMessage.Chat.Id);

            _logger.Info($"Incoming message from {inputMessage.Chat.Id}. Message content: \"{inputMessage.Text}\"");
            var user = GetOrCreateUser(inputMessage.Chat.Id);

            if (_messageHandlers.ContainsKey(user.State))
                resultMessage = _messageHandlers[user.State].GetResponseTo(inputMessage, user);

            if (_commandHandlers.ContainsKey(inputMessage.Text))
                resultMessage = _commandHandlers[inputMessage.Text].GetResponseTo(inputMessage, user);

            if (resultMessage == null)
                resultMessage = BaseTgMessageHandler.GetDefaultResponse(inputMessage.Chat.Id);

            await _messageSender.SendMessageToUser(resultMessage);
        }

    }
}
