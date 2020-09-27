using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using WhisleBotConsole.BotContorller;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;

namespace WhisleBotConsole.Vk
{
    class UserNewMentionsNotifier : IUserNotifier
    {
        private readonly Logger _logger;
        private readonly IMessageSender _messageSender;
        private readonly UsersContext _db;

        public UserNewMentionsNotifier(IMessageSender messageSender, UsersContext db)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _messageSender = messageSender;
            _db = db;
        }


        public void NotifyUser(long userId, long groupId, string groupName, long postId, string keyword)
        {
            var user = _db.Users.Where(user => user.Id == userId).FirstOrDefault();
            if (user == null)
            {
                _logger.Error($"Cannot find user with id:{userId}. Found keyword:{keyword}");
            }

            var message = new TelegramUserMessage()
            {
                ChatId = user.ChatId,
                Text = $"В группе id:{groupId} {groupName ?? string.Empty} В посте {postId}  упоминается слово _{keyword}_. [Ссылка](https://vk.com/wall-{groupId}_{postId}/) "
            };

            _logger.Info($"Notifying user {user.Username} {user.Title} (id: {user.Id}) with text {message.Text}");
            _messageSender.SendMessageToUser(message);
        }
    }
}
