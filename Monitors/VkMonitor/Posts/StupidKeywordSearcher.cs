using VkNet.Model.Attachments;

namespace Wbcl.Monitors.VkMonitor.Posts
{
    public class StupidKeywordSearcher : IPostKeywordSearcher
    {
        public (bool Contains, string Word) LookIntoPost(Post post, string[] keywords)
        {
            if (post == null)
                return (false, string.Empty);
            if (string.IsNullOrEmpty(post.Text))
                return (false, string.Empty);

            var text = post.Text.ToLower();
            foreach (var keyword in keywords)
            {
                if (text.Contains(keyword))
                    return (true, keyword);
            }

            return (false, string.Empty);
        }
    }
}
