using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.Extensions;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MarkupUtils;

namespace WhisleBotConsole.TelegramBot.MessageHandlers.AdminCommands
{
    class SetUserStatus : BaseTgMessageHandler
    {
        private readonly Settings _settings;

        public SetUserStatus(UsersContext _db, IOptions<Settings> settings)
            :base(_db)
        {
            _settings = settings.Value;
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, DB.User user)
        {
            var result = string.Empty;
            var commandLine = inputMessage.Text.Replace(UsedUserInput, string.Empty);

            var commandParams = commandLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (commandParams.Length != 4)            
                return GetDefaultResponse(inputMessage.Chat.Id, "Incorect input provided. Expected 3 parameters after command");
            
            if (!int.TryParse(commandParams[1], out int userID))            
                return GetDefaultResponse(inputMessage.Chat.Id, $"Incorect input provided. Cannot convert 1st parameter {commandParams[1]}");
            
            if (!int.TryParse(commandParams[2], out int userStatus))            
                return GetDefaultResponse(inputMessage.Chat.Id, $"Incorect input provided. Cannot convert 2nd parameter {commandParams[2]}");
            
            if (!DateTime.TryParse(commandParams[3], out DateTime endDate))            
                return GetDefaultResponse(inputMessage.Chat.Id, $"Incorect input provided. Cannot convert 3rd parameter {commandParams[3]}");
            

            var neededUser = _db.Users.FirstOrDefault(usr => usr.Id == userID);

            if (neededUser == null)
                return GetDefaultResponse(inputMessage.Chat.Id, $"Canot find a user with id {userID}");

            neededUser.SubscriptionStatus = (UserType)userStatus;
            neededUser.EndOfAdvancedSubscription = endDate;
            _db.SaveChanges();

            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = $"New user info: {neededUser.ToShortString()}",
                ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
            };
        }

        public override ChatState UsedChatState => ChatState.NotUsed;
        public override string UsedUserInput => $"{_settings.Telegram.AdminKeyword} || setUserStatus ";
    }
}
