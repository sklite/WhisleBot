using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using Wbcl.Core.Models.Database;

namespace Wbcl.Clients.TgClient.MarkupUtils
{
    public static class MessageMarkupUtilities
    {
        public static List<List<KeyboardButton>> GetReplyKeyboardForGroups(IEnumerable<UserPreference> groups)
        {
            var buttons = groups.Select(pref => new KeyboardButton[] { $"{pref.TargetName} (id: {pref.TargetId})" });
            var keyboard = new List<List<KeyboardButton>>();
            foreach (var button in buttons)
            {
                keyboard.Add(button.ToList());
            }
            return keyboard;
        }

        public static ReplyKeyboardMarkup GetDefaultMarkup()
        {
            return new ReplyKeyboardMarkup(
                                new KeyboardButton[][]
                                {
                                        new KeyboardButton[] { TgBotText.AddNewSettings },
                                        new KeyboardButton[] { TgBotText.EditExistingSettings, TgBotText.RemoveSubscriptions },
                                },
                                resizeKeyboard: true
                            );
        }
    }
}
