using System;
using System.Text;
using Telegram.Bot.Types;
using Wbcl.Clients.TgClient.MarkupUtils;
using Wbcl.Clients.TgClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.Core.Models.Settings;
using Wbcl.Core.Utils;
using Wbcl.DAL.Context;

namespace Wbcl.Clients.TgClient.MessageHandlers.AdminCommands
{
    class GetAllUsers : BaseTgMessageHandler
    {
        private readonly Settings _settings;

        public GetAllUsers(IUsersContext db, Settings settings)
            :base(db)
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
