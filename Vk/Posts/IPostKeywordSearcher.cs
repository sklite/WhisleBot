using VkNet.Model.Attachments;

namespace WhisleBotConsole.Vk.Posts
{
    interface IPostKeywordSearcher
    {
        (bool Contains, string Word) LookIntoPost(Post post, string[] keywords);
    }
}