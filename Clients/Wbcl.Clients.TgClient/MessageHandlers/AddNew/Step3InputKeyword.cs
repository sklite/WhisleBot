using System;
using System.Linq;
using Telegram.Bot.Types;
using Wbcl.Clients.TgClient.MarkupUtils;
using Wbcl.Clients.TgClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.Core.Models.Settings;
using Wbcl.DAL.Context;
using User = Wbcl.Core.Models.Database.User;

namespace Wbcl.Clients.TgClient.MessageHandlers.AddNew
{
    class Step3InputKeyword : BaseTgMessageHandler
    {
        private readonly Settings _settings;

        public Step3InputKeyword(IUsersContext _db, Settings settings)
            : base(_db)
        {
            _settings = settings;
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

            if (user.CurrentTargetId == null)
            {
                user.CurrentTargetId = null;
                user.CurrentTargetName = null;
                return FailWithText(inputMessage.Chat.Id, user, @"Что-то пошло не так\. Попробуйте ещё раз или позже");
            }

            var userPrefs = _db.Preferences.Where(pref => pref.User.Id == user.Id && pref.TargetId == user.CurrentTargetId).FirstOrDefault();

            if (userPrefs == null)
            {
                userPrefs = new UserPreference()
                {
                    User = user,
                    TargetId = user.CurrentTargetId.Value,
                    TargetName = user.CurrentTargetName,
                    Keyword = inputMessage.Text,
                    LastNotifiedPostTime = DateTime.MinValue,
                    TargetType = user.CurrentTargetType.Value
                };
                _db.Preferences.Add(userPrefs);
            }
            else
            {
                userPrefs.Keyword = inputMessage.Text;
            }

            user.CurrentTargetId = null;
            user.CurrentTargetName = null;
            user.CurrentTargetType = null;
            user.State = ChatState.Standrard;

            _db.SaveChanges();

            return new TelegramUserMessage()
            {
                ChatId = inputMessage.Chat.Id,
                Text = @$"Отлично. Слова записаны. Когда в группе *{userPrefs.TargetName}* (id:_{userPrefs.TargetId}_) появятся новые посты со следюующими словами: _{inputMessage.Text}_ Вы получите уведомление сюда",
                ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
            };
        }

        public override ChatState UsedChatState => ChatState.NewWordToGroupAdd;
        public override string UsedUserInput => string.Empty;
    }
}
