using System;
using Wbcl.Core.Models.Database;

namespace Wbcl.Core.Utils
{
    public static class FormatExtensions
    {
        public static string ToShortString(this User user)
        {
            var userInfo = $"Id: {user.Id}; " +
                    $"ChatId: {user.ChatId}; " +
                    $"Username: {user.Username}; " +
                    $"Title: {user.Title}; " +
                    $"Status: {(int)user.SubscriptionStatus}; " +
                    $"Till: {user.EndOfAdvancedSubscription.ToShortDateString()}";

            return userInfo;
        }

        public static string ToShortString(this UserPreference pref)
        {
            return $"{nameof(pref.Id)}: {pref.Id};" + Environment.NewLine +
                $"{nameof(pref.Keyword)}: {pref.Keyword};" + Environment.NewLine +
                $"{nameof(pref.TargetId)}: {pref.TargetId};" + Environment.NewLine +
                $"{nameof(pref.LastNotifiedPostTime)}: {pref.LastNotifiedPostTime};" + Environment.NewLine +
                $"{nameof(pref.User)}: {(pref.User == null ? string.Empty : pref.User.Id.ToString())};" + Environment.NewLine;
        }
    }

}
