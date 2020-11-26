using System;
using System.Collections.Generic;
using System.Text;

namespace WhisleBotConsole.Config
{
    class VkSettings
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public ulong AppId { get; set; }
        public int BaseSearchInterval { get; set; }
        public int BaseSubscriptionsLimit { get; set; }
        public int ExtendedSubscriptionsLimit { get; set; }
        public int KeywordCharacterLimit { get; set; }
    }
}
