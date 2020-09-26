using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using WhisleBotConsole.Models;

namespace WhisleBotConsole.BotContorller
{
    class TelegramMessageSender : IMessageSender
    {
        readonly ITelegramBotClient _botClient;

        public TelegramMessageSender(ITelegramBotClient botCLient)
        {
            _botClient = botCLient;
        }

        public async Task SendMessageToUser(IMessage message)
        {
            if (message == null)
                return;

            var tgMessage = message as TelegramUserMessage;

            await _botClient.SendTextMessageAsync(
                tgMessage.ChatId,
                tgMessage.Text,
                Telegram.Bot.Types.Enums.ParseMode.Markdown,
                replyMarkup: tgMessage.ReplyMarkup);
        }
    }
}
