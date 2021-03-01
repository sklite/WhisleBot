using VkNet.Model.Attachments;

namespace Wbcl.Monitors.VkMonitor.Posts
{
    public interface IPostKeywordSearcher
    {
        (bool Contains, string Word) LookIntoPost(Post post, string[] keywords);
    }
}