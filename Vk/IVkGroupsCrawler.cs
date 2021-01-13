using System;
using System.Collections.Generic;
using System.Text;
using WhisleBotConsole.DB;

namespace WhisleBotConsole.Vk
{
    interface IVkGroupsCrawler
    {
        void DoSearch();
        (bool Success, long Id, string Name, PreferenceType LinkType) GetObjIdIdByLink(Uri link);
    }
}
