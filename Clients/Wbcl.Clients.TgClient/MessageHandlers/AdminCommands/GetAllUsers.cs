using Microsoft.Extensions.Options;
using System;
using System.Text;
using Telegram.Bot.Types;
using Wbcl.Clients.TelegramClient.MessageHandlers;
using Wbcl.Clients.TelegramClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.Core.Models.Settings;
using Wbcl.Core.Utils;
using Wbcl.DAL.Context;
using WhisleBotConsole.TelegramBot.MarkupUtils;

namespace WhisleBotConsole.TelegramBot.MessageHandlers.AdminCommands
{
    class GetAllUsers : BaseTgMessageHandler
    {
        private readonly Settings _settings;

        public GetAllUsers(IUsersContext _db, Settings settings)
            :base(_db)
        {
            _settings = settings;
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, Wbcl.Core.Models.Database.User user)
        {
            var sb = new StringBuilder();
            foreach (var dbUser in _db.Users)
            {
                sb.AppendLine(FormatExtensions.ToShortString(dbUser));
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
