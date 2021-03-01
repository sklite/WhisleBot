using Microsoft.Extensions.Options;
using System;
using Telegram.Bot.Types;
using Wbcl.Clients.TelegramClient.MessageHandlers;
using Wbcl.Clients.TelegramClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.Core.Models.Settings;
using Wbcl.DAL.Context;
using WhisleBotConsole.TelegramBot.MarkupUtils;

namespace WhisleBotConsole.TelegramBot.MessageHandlers.AdminCommands
{
    class DownloadDbFile : BaseTgMessageHandler
    {
        private readonly Settings _settings;

        public DownloadDbFile(IUsersContext _db, Settings settings)
            : base(_db)
        {
            _settings = settings;
        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, Wbcl.Core.Models.Database.User user)
        {
            try
            {
                //var dbFilePath = IUsersContext.GetDbFilePath();


                //string dbCopyFileName = "users-copy.db";
                //string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //var tempFile = Path.Combine(currentPath, dbCopyFileName);
                //if (System.IO.File.Exists(dbCopyFileName))
                //{
                //    System.IO.File.Delete(dbCopyFileName);
                //}
                //System.IO.File.Copy(dbFilePath, dbCopyFileName);


                //var file = System.IO.File.OpenRead(dbCopyFileName);
                //var tgFile = new InputOnlineFile(file, "users.db");


                return new TelegramUserMessage()
                {
                    ChatId = inputMessage.Chat.Id,
                    Text = $"Функци не реализована!",
                 //   File = tgFile,
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
