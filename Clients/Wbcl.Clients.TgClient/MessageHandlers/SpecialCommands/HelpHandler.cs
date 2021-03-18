using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;
using Wbcl.Clients.TelegramClient.MessageHandlers;
using Wbcl.Clients.TelegramClient.Models;
using Wbcl.Core.Models.Database;
using Wbcl.DAL.Context;
using WhisleBotConsole.TelegramBot.MarkupUtils;

namespace Wbcl.Clients.TgClient.MessageHandlers.SpecialCommands
{
    class HelpHandler : BaseTgMessageHandler
    {
        public override ChatState UsedChatState => ChatState.NotUsed;
        public override string UsedUserInput => "/help";

        public HelpHandler(IUsersContext db)
            :base(db)
        {

        }

        public override TelegramUserMessage GetResponseTo(Message inputMessage, Core.Models.Database.User user)
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
                    Text = @$"Данный бот поможет вам следить за группами вк без лишних телодвижений.🦾" +

                    "\r\n\r\nНапример, вы занимаетесь ремонтом в Москве и ищете клиентов.Пусть бот это делает за вас! Подпишитесь на группы \"Подслушано\" вашего района," +
                    "укажите слова \"_ремонт, сантехника, специалист, электрика_\".Как только в группах появятся новые посты с ключевыми словами," +
                    "вы уже будете об этом знать и клиент будет ваш!" +

                    "\r\n\r\nДругой сценарий - вы хотите подешевле снять хорошую однушку на севере Москвы.🏡 Но как назло все самые вкусные объявления уже сданы.Не время опускать руки! Скажите боту мониторить слова \"_однушка, однокомнатная_\" в группе \"Недвижимость САО\".И всё! Следующее объявление о сдаче нужной квартиры точно не пройдёт мимо." +

                    "\r\n\r\nВы всегда будете в курсе нужных вам групп, а бот сэкономит вам кучу времени. Это ли не счастье? ",
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
    }
}
