using Microsoft.Extensions.Options;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WhisleBotConsole.BotContorller;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MessageHandlers;
using WhisleBotConsole.TelegramBot.MessageHandlers.AdminCommands;
using WhisleBotConsole.Vk;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot
{
    class TelegramMessageRouter : ITelegramMessageRouter
    {
        Dictionary<ChatState, BaseTgMessageHandler> _messageHandlers;
        Dictionary<string, BaseTgMessageHandler> _commandHandlers;
        List<BaseTgMessageHandler> _myMessageHandlers;

        private readonly UsersContext _db;
        private readonly IMessageSender _messageSender;
        private readonly Logger _logger;

        public TelegramMessageRouter(UsersContext db, IVkGroupsCrawler vk, IMessageSender messageSender, IOptions<Settings> settings)
        {
            _db = db;
            _messageSender = messageSender;
            _logger = LogManager.GetCurrentClassLogger();

            _myMessageHandlers = new List<BaseTgMessageHandler>
            {
                new Step2InputGroup(_db, vk),
                new Step3InputKeyword(_db, settings),
                new UpdateKeywords(_db),
                new RemoveSettingsStep2(_db),
                new Step1AddNewAlarms(_db, settings),
                new EditExistingSettings(_db, vk),
                new RemoveSettingsStep1(_db, vk),
                new GetAllUsers(_db, settings),
                new SetUserStatus(_db, settings),
                new DownloadDbFile(_db, settings)
            };
        }

        User DefineUser(Chat tgChat)
        {
            User user = null;
            try
            {
                user = _db.Users.Where(user => user.ChatId == tgChat.Id).FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                _logger.Error($"Caught exception {ex}");
            }
            
            if (user == null)
            {
                user = new DB.User() { ChatId = tgChat.Id, Username = tgChat.Username, Title = tgChat.Title };
                _db.Users.Add(user);
                _logger.Info("Saving changes..");
                _db.SaveChanges();
            }

            if (user.Username != tgChat.Username || user.Title != $"{tgChat.FirstName} {tgChat.LastName}")
            {
                user.Username = tgChat.Username;
                user.Title = $"{tgChat.FirstName} {tgChat.LastName}";
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
            var user = DefineUser(inputMessage.Chat);

            var userInput = inputMessage.Text.Split("|||", System.StringSplitOptions.RemoveEmptyEntries);

            var handler = _myMessageHandlers.FirstOrDefault(han => han.UsedChatState == user.State) 
                    ?? _myMessageHandlers.FirstOrDefault(han => han.UsedUserInput == userInput.FirstOrDefault());

            resultMessage = handler != null 
                ? handler.GetResponseTo(inputMessage, user) 
                : BaseTgMessageHandler.GetDefaultResponse(inputMessage.Chat.Id);

            await _messageSender.SendMessageToUser(resultMessage);
        }

    }
}
