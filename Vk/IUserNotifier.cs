﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhisleBotConsole.Vk
{
    interface IUserNotifier
    {
        void NotifyUser(long iserId, long groupId, string groupName, long postId, string keyword);
    }
}
