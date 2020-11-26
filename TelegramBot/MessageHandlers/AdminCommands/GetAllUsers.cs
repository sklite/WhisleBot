using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.Extensions;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MarkupUtils;

namespace WhisleBotConsole.TelegramBot.MessageHandlers.AdminCommands
{
    class GetAllUsers : BaseTgMessageHandler
    {
        private readonly Settings _settings;

        public GetAllUsers(UsersContext _db, IOptions<Settings> settings)
            :base(_db)
        {
            _settings = settings.Value;
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, DB.User user)
        {
            var sb = new StringBuilder();
            foreach (var dbUser in _db.Users)
            {
                sb.AppendLine(dbUser.ToChatString());
            }

            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = $"Список пользователей: {Environment.NewLine} {sb}",
                ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
            };
        }

        public override ChatState UsedChatState => ChatState.NotUsed;
        public override string UsedUserInput => $"{_settings.Telegram.AdminKeyword} || userlist";
    }
}
