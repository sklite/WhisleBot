using System;
using System.Collections.Generic;
using System.Text;

namespace WhisleBotConsole.Vk
{
    interface IVkGroupsSearcher
    {
        (bool Success, string Result) GetPostLinkByKeyword(string keyword);
        (bool Success, long GroupId, string GroupName) GetGroupIdByLink(Uri link);
    }
}
