using Telegram.Bot.Types;
using Wbcl.Clients.TgClient.MarkupUtils;
using Wbcl.Clients.TgClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.DAL.Context;
using User = Wbcl.Core.Models.Database.User;

namespace Wbcl.Clients.TgClient.MessageHandlers
{
    public abstract class BaseTgMessageHandler
    {
        protected readonly IUsersContext _db;

        public BaseTgMessageHandler(IUsersContext db)
        {
            _db = db;
        }

        public abstract TelegramUserMessage GetResponseTo(Message inputMessage, User user);

        protected TelegramUserMessage FailWithText(long chatId, User user, string text)
        {
            user.State = ChatState.Standrard;
            _db.SaveChanges();
            return GetDefaultResponse(chatId, text);
        }

        public static TelegramUserMessage GetDefaultResponse(long chatId, string additionalText = "")
        {
            return new TelegramUserMessage()
            {
                ChatId = chatId,
                Text = $"{additionalText}\nВыбери что тебе нужно сделать:",
                ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
            };
        }

        /// <summary>
        /// A chat state, where this handler will fire (has priority over the UsedUserInput)
        /// </summary>
        public abstract ChatState UsedChatState { get; }

        /// <summary>
        /// The user input, which is needed for handling the message
        /// </summary>
        public abstract string UsedUserInput { get; }
    }
}
