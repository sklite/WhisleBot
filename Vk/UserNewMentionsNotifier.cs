using NLog;
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


        public void NotifyUser(UserPreference preference, long postId, string keyword)
        {
            if (preference == null)
            {
                _logger.Error($"Null preference settings. Found keyword:{keyword}");
                return;
            }
            var user = preference.User;

            if (user == null)
            {
                _logger.Error($"Cannot find user for preferenceId:{preference.Id}. Found keyword:{keyword}");
                return;
            }

            var messageText = preference.TargetType == PreferenceType.VkGroup
               ? $"В группе *{preference.TargetName}* (id:{preference.TargetId}) В [посте](https://vk.com/wall-{preference.TargetId}_{postId}/) упоминается _{keyword}_. "
               : $"На стене пользователя *{preference.TargetName}* (id:{preference.TargetId}) В [посте](https://vk.com/wall{preference.TargetId}_{postId}/) упоминается _{keyword}_. ";

            var message = new TelegramUserMessage()
            {
                ChatId = user.ChatId,
                Text = messageText
            };
                        
            _messageSender.SendMessageToUser(message);
        }
    }
}
