using System;
using System.Collections.Generic;
using System.Text;
using Wbcl.Core.Models.Database;

namespace Wbcl.DAL.Context
{
    public interface IHistoryLogger
    {
        bool LogHistory(long chatId, DateTime time, bool toUser, string text);
        bool LogHistory(IUsersContext db, User user, DateTime time, bool toUser, string text);
    }
}
