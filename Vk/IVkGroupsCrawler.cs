using System;
using System.Collections.Generic;
using System.Text;

namespace WhisleBotConsole.Vk
{
    interface IVkGroupsCrawler
    {
        void DoSearch();
        (bool Success, long GroupId, string GroupName) GetGroupIdByLink(Uri link);
    }
}
