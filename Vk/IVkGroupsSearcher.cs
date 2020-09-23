using System;
using System.Collections.Generic;
using System.Text;

namespace WhisleBotConsole.Vk
{
    interface IVkGroupsSearcher
    {
        (bool Success, long GroupId, string GroupName) GetGroupIdByLink(Uri link);
        void StartSearch(int interval);
        void StopSearch();
    }
}
