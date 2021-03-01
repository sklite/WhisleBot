using Wbcl.Core.Models.Database;

namespace Wbcl.Monitors.VkMonitor
{
    public interface IUserNotifier
    {
        void NotifyUser(UserPreference preference, long postId, string keyword);
    }
}
