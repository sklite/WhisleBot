using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhisleBotConsole.Config;

namespace WhisleBotConsole.Config
{
    class Settings
    {
        public TelegramSettings Telegram { get; set; }
        public VkSettings Vkontakte { get; set; }
        public DbSettings DbSettings { get; set; }
    }
}
