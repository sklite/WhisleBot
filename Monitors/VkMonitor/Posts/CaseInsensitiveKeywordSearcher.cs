using Cyriller;
using Cyriller.Model;
using VkNet.Model.Attachments;

namespace Wbcl.Monitors.VkMonitor.Posts
{
    public class CaseInsensitiveKeywordSearcher : IPostKeywordSearcher
    {
        private readonly CyrNounCollection _nounCollection;

        public CaseInsensitiveKeywordSearcher(CyrNounCollection nounCollection)
        {
            _nounCollection = nounCollection;
        }

        public (bool Contains, string Word) LookIntoPost(Post post, string[] keywords)
        {
            if (post == null)
                return (false, string.Empty);
            if (string.IsNullOrEmpty(post.Text))
                return (false, string.Empty);

            var text = post.Text.ToLower();
            foreach (var keywordItem in keywords)
            {
                var keyword = keywordItem.Trim();
                try
                {
                    var original = _nounCollection.Get(keyword, out CasesEnum @case, out NumbersEnum number);
                    if (original == null)
                    {
                        if (SimpleCheck(text, keyword))
                            return (true, keyword);
                        continue;
                    }

                    var declined = original.Decline();
                    if (declined == null)
                    {
                        if (SimpleCheck(text, keyword))
                            return (true, keyword);
                        continue;
                    }

                    var allCases = declined.ToList();

                    foreach (var caseItem in allCases)
                    {
                        if (SimpleCheck(text, caseItem))
                        {
                            return (true, caseItem);
                        }
                    }
                }
                catch (CyrWordNotFoundException)
                {
                    if (SimpleCheck(text, keyword))
                    {
                        return (true, keyword);
                    }
                }

            }

            return (false, string.Empty);
        }

        bool SimpleCheck(string postText, string keyword)
        {
            return postText.Contains(keyword.ToLower());
        }
    }
}
