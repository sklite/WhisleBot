using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Wbcl.Clients.TelegramClient.MessageHandlers;
using Wbcl.Clients.TgClient;
using Wbcl.Core.Models.Notifications;
using Wbcl.Core.Models.Services;
using Wbcl.Core.Models.Settings;
using Wbcl.Core.Utils;
using Wbcl.DAL.Context;
using WhisleBotConsole.TelegramBot.MessageHandlers;
using WhisleBotConsole.TelegramBot.MessageHandlers.AdminCommands;
using User = Wbcl.Core.Models.Database.User;

namespace WhisleBotConsole.TelegramBot
{
    public class TelegramMessageRouter : ITelegramMessageRouter
    {
        private readonly List<BaseTgMessageHandler> _myMessageHandlers;

        private readonly IUsersContext _db;
        private readonly IHistoryLogger _historyLogger;
        private readonly IMessageSender _messageSender;
        private readonly Logger _logger;

        public TelegramMessageRouter(IUsersContext db, IHistoryLogger historyLogger, IMessageSender messageSender, IVkUtils vk, Settings settings)
        {
            _db = db;
            _historyLogger = historyLogger;
            _messageSender = messageSender;
            _logger = LogManager.GetCurrentClassLogger();

            _myMessageHandlers = new List<BaseTgMessageHandler>
            {
                new Step2InputGroup(_db, vk),
                new Step3InputKeyword(_db, settings),
                new UpdateKeywords(_db),
                new RemoveSettingsStep2(_db),
                new Step1AddNewAlarms(_db, settings),
                new EditExistingSettings(_db),
                new RemoveSettingsStep1(_db),
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
                user = new User() { ChatId = tgChat.Id, Username = tgChat.Username, Title = tgChat.Title };
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
            try
            {
                IMessage resultMessage = null;

                if (inputMessage == null || string.IsNullOrEmpty(inputMessage.Text))
                    resultMessage = BaseTgMessageHandler.GetDefaultResponse(inputMessage.Chat.Id);

                _logger.Info($"Incoming message from {inputMessage.Chat.Id}. Message content: \"{inputMessage.Text}\"");
                var user = DefineUser(inputMessage.Chat);
                _historyLogger.LogHistory(inputMessage.Chat.Id, DateTime.Now, false, inputMessage.Text);

                var userInput = inputMessage.Text.Split("|||", System.StringSplitOptions.RemoveEmptyEntries);

                var handler = _myMessageHandlers.FirstOrDefault(han => han.UsedUserInput == userInput.FirstOrDefault()) ??
                    _myMessageHandlers.FirstOrDefault(han => han.UsedChatState == user.State);

                resultMessage = handler != null
                    ? handler.GetResponseTo(inputMessage, user)
                    : BaseTgMessageHandler.GetDefaultResponse(inputMessage.Chat.Id);

                await _messageSender.SendMessageToUser(resultMessage);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Exception happened in TelegramMessageRouter. {ex}");
            }

        }

    }
}
