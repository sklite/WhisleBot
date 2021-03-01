using System;
using System.Linq;
using Wbcl.Core.Models.Database;

namespace Wbcl.DAL.Context
{
    class HistoryLogger : IHistoryLogger
    {
        private readonly IUsersContext _db;

        public HistoryLogger(IUsersContext db)
        {
            _db = db;
        }

        public bool LogHistory(long chatId, DateTime time, bool toUser, string text)
        {
            var user = _db.Users.Where(user => user.ChatId == chatId).FirstOrDefault();
            if (user == null)
                return false;

            return LogHistory(_db, user, time, toUser, text);
        }

        /// <summary>
        /// Something wrong with saving changes - trying to save a new user as well 
        /// </summary>
        /// <returns></returns>
        public bool LogHistory(IUsersContext db, User user, DateTime time, bool toUser, string text)
        {
            var history = new ChatHistoryItem()
            {
                MessageText = text,
                ToUser = toUser,
                User = user,
                Sent = time,
            };

            db.ChatHistory.Add(history);
            db.SaveChanges();
            return true;
        }
    }
}
