using WhisleBotConsole.DB;

namespace WhisleBotConsole.Extensions
{
    static class FormatExtensions
    {
        public static string ToChatString(this User user)
        {
            var userInfo = $"Id: {user.Id}; " +
                    $"ChatId: {user.ChatId}; " +
                    $"Username: {user.Username}; " +
                    $"Status: {(int)user.SubscriptionStatus}; " +
                    $"Till: {user.EndOfAdvancedSubscription.ToShortDateString()}";

            return userInfo;
        }
    }

}
