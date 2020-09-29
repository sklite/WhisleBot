using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhisleBotConsole.DB;

namespace WhisleBotConsole.Vk
{
    interface IUserNotifier
    {
        void NotifyUser(UserPreference preference, long postId, string keyword);
    }
}
