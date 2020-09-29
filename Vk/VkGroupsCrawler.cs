using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Linq;
using System.Threading;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;
using WhisleBotConsole.DB;
using WhisleBotConsole.Vk.Posts;

namespace WhisleBotConsole.Vk
{
    class VkGroupsCrawler : IVkGroupsCrawler
    {
        private readonly IVkApi _api;        
        private readonly Logger _logger;
        private readonly UsersContext _usersContext;
        private readonly IPostKeywordSearcher _keywordSearcher;
        private readonly IUserNotifier _userNotifier;


        public VkGroupsCrawler(UsersContext usersContect,
            IPostKeywordSearcher keywordSearcher,
            IUserNotifier userNotifier,
            IVkApi vkApi)
        {
            _api = vkApi;
            _logger = LogManager.GetCurrentClassLogger();
            _usersContext = usersContect;
            _keywordSearcher = keywordSearcher;
            _userNotifier = userNotifier;
        }
        private string[] PrepareKeywords(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return null;
            var keywords = keyword.ToLower().Split(',');
            return keywords;
        }
        public (bool Success, long GroupId, string GroupName) GetGroupIdByLink(Uri link)
        {
            try
            {
                var groupIdentifier = link.PathAndQuery.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();
                var vkObj = _api.Utils.ResolveScreenName(groupIdentifier);
                if (vkObj == null || vkObj.Type != VkNet.Enums.VkObjectType.Group)
                    return (false, -1, string.Empty);

                var groupInfo = _api.Groups.GetById(null, vkObj.Id.Value.ToString(), VkNet.Enums.Filters.GroupsFields.Description);
                if (groupInfo == null || !groupInfo.Any())
                    return (false, -1, string.Empty);

                return (true, vkObj.Id.Value, groupInfo.First().Name);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return (false, -1, string.Empty);
            }

        }

        public void DoSearch()
        {
            var allPrefs = _usersContext.Preferences.Include(u => u.User);
            foreach (var prefs in allPrefs)
            {
                if (ValidForSearch(prefs))
                {
                    var keywords = PrepareKeywords(prefs.Keyword);
                    var wallGeParams = new WallGetParams
                    {
                        Count = 5,
                        OwnerId = -prefs.GroupId
                    };
                    Thread.Sleep(1000);
                    var getResult = _api.Wall.Get(wallGeParams);
                    var posts = getResult.WallPosts;

                    foreach (var post in posts)
                    {
                        var searchResult = _keywordSearcher.LookIntoPost(post, keywords);
                        if (!searchResult.Contains)
                            continue;

                        if (post.Date <= prefs.LastNotifiedPostTime)
                            continue;

                        prefs.LastNotifiedPostTime = post.Date ?? DateTime.Now;
                        _userNotifier.NotifyUser(prefs, post.Id.Value, searchResult.Word);
                    }

                }
            }
            _usersContext.SaveChanges();
        }

        bool ValidForSearch(UserPreference prefs)
        {
            return (prefs.GroupId > 0
                && !string.IsNullOrEmpty(prefs.Keyword));
        }
    }

}
