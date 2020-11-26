using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Telegram.Bot.Types;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MarkupUtils;
using User = WhisleBotConsole.DB.User;

namespace WhisleBotConsole.TelegramBot.MessageHandlers
{
    class Step3InputKeyword : BaseTgMessageHandler
    {
        private readonly Settings _settings;

        public Step3InputKeyword(UsersContext _db, IOptions<Settings> settings)
            : base(_db)
        {
            _settings = settings.Value;
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, User user)
        {
            if (string.IsNullOrEmpty(inputMessage.Text))
            {
                return FailWithText(inputMessage.Chat.Id, user, "Пустой текст не является ключевым словом");
            }

            if (inputMessage.Text.Length > _settings.Vkontakte.KeywordCharacterLimit)
            {
                return FailWithText(inputMessage.Chat.Id, user, $"Введён слишком длинный текст. Текущий лимит {_settings.Vkontakte.KeywordCharacterLimit} символов.");
            }

            if (user.CurrentGroupId == null)
            {
                user.CurrentGroupId = null;
                user.CurrentGroupName = null;
                return FailWithText(inputMessage.Chat.Id, user, @"Что-то пошло не так\. Попробуйте ещё раз или позже");
            }

            var userPrefs = _db.Preferences.Where(pref => pref.User.Id == user.Id && pref.GroupId == user.CurrentGroupId).FirstOrDefault();

            if (userPrefs == null)
            {
                userPrefs = new UserPreference()
                {
                    User = user,
                    GroupId = user.CurrentGroupId.Value,
                    GroupName = user.CurrentGroupName,
                    Keyword = inputMessage.Text,
                    LastNotifiedPostTime = DateTime.MinValue
                };
                _db.Preferences.Add(userPrefs);
            }
            else
            {
                userPrefs.Keyword = inputMessage.Text;
            }

            user.CurrentGroupId = null;
            user.CurrentGroupName = null;
            user.State = ChatState.Standrard;

            _db.SaveChanges();

            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = @$"Отлично. Слова записаны. Когда в группе *{userPrefs.GroupName}* (id:_{userPrefs.GroupId}_) появятся новые посты со следюующими словами: _{inputMessage.Text}_ Вы получите уведомление сюда",
                ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
            };
        }

        public override ChatState UsedChatState => ChatState.NewWordToGroupAdd;
        public override string UsedUserInput => string.Empty;
    }
}
