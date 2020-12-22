using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.Extensions;
using WhisleBotConsole.Models;
using WhisleBotConsole.TelegramBot.MarkupUtils;

namespace WhisleBotConsole.TelegramBot.MessageHandlers.AdminCommands
{
    class DownloadDbFile : BaseTgMessageHandler
    {
        private readonly Settings _settings;

        public DownloadDbFile(UsersContext _db, IOptions<Settings> settings)
            : base(_db)
        {
            _settings = settings.Value;
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, DB.User user)
        {
            try
            {
                var dbFilePath = UsersContext.GetDbFilePath();


                string dbCopyFileName = "users-copy.db";
                string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var tempFile = Path.Combine(currentPath, dbCopyFileName);
                if (System.IO.File.Exists(dbCopyFileName))
                {
                    System.IO.File.Delete(dbCopyFileName);
                }
                System.IO.File.Copy(dbFilePath, dbCopyFileName);


                var file = System.IO.File.OpenRead(dbCopyFileName);
                var tgFile = new InputOnlineFile(file, "users.db");


                return new TelegramUserMessage()
                {
                    ChatId = inputMessage.Chat.Id,
                    Text = $"Вот файл",
                    File = tgFile,
                    ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
                };
            }
            catch (Exception ex)
            {
                return new TelegramUserMessage()
                {
                    ChatId = inputMessage.Chat.Id,
                    Text = $"Произошла ошибка: {ex.Message}, Ошибка: {ex}",
                    ReplyMarkup = MessageMarkupUtilities.GetDefaultMarkup()
                };
            }

        }

        public override ChatState UsedChatState => ChatState.NotUsed;
        public override string UsedUserInput => $"{_settings.Telegram.AdminKeyword} || downloadDb";
    }
}
