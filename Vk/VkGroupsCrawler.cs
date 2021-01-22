using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Linq;
using System.Threading;
using VkNet.Abstractions;
using VkNet.Exception;
using VkNet.Model.RequestParams;
using WhisleBotConsole.DB;
using WhisleBotConsole.Vk.Posts;
using WhisleBotConsole.Vk.Extensions;
using Microsoft.Extensions.Options;
using WhisleBotConsole.Config;
using System.Collections.Generic;
using VkNet.Enums;
using WhisleBotConsole.Extensions;
using VkNet.Model;

namespace WhisleBotConsole.Vk
{
    class VkGroupsCrawler : IVkGroupsCrawler
    {
        private readonly IVkApi _api;
        private readonly IOptions<Settings> _settings;
        private readonly Logger _logger;
        private readonly UsersContext _usersContext;
        private readonly IPostKeywordSearcher _keywordSearcher;
        private readonly IUserNotifier _userNotifier;
        private readonly List<VkObjectType> _supportedVkTypes;

        public VkGroupsCrawler(UsersContext usersContect,
            IPostKeywordSearcher keywordSearcher,
            IUserNotifier userNotifier,
            IVkApi vkApi,
            IOptions<Settings> settings)
        {
            _api = vkApi;
            _settings = settings;
            _logger = LogManager.GetCurrentClassLogger();
            _usersContext = usersContect;
            _keywordSearcher = keywordSearcher;
            _userNotifier = userNotifier;
            _supportedVkTypes = new List<VkObjectType> { VkObjectType.Group, VkObjectType.User };
        }

        private string[] PrepareKeywords(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return null;
            var keywords = keyword.ToLower().Split(',');
            return keywords;
        }

        public (bool Success, long Id, string Name, PreferenceType LinkType) GetObjIdIdByLink(Uri link)
        {
            try
            {
                var groupIdentifier = link.LocalPath.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();
                var vkObj = _api.Utils.ResolveScreenName(groupIdentifier);
                if (vkObj == null || !_supportedVkTypes.Contains(vkObj.Type))
                    return (false, -1, string.Empty, PreferenceType.WrongLink);

                switch (vkObj.Type)
                {
                    case VkObjectType.Group:
                        var groupInfo = _api.Groups.GetById(null, vkObj.Id.Value.ToString(), VkNet.Enums.Filters.GroupsFields.Description);
                        if (groupInfo == null || !groupInfo.Any())
                            return (false, -1, string.Empty, PreferenceType.WrongLink);

                        return (true, vkObj.Id.Value, groupInfo.First().Name, PreferenceType.VkGroup);

                    case VkObjectType.User:
                        var userInfo = _api.Users.Get(new List<long> { vkObj.Id.Value }, VkNet.Enums.Filters.ProfileFields.FirstName | VkNet.Enums.Filters.ProfileFields.LastName);
                        if (userInfo == null || !userInfo.Any())
                            return (false, -1, string.Empty, PreferenceType.WrongLink);

                        return (true, vkObj.Id.Value, $"{userInfo.First().FirstName} {userInfo.First().LastName}", PreferenceType.VkUser);

                    default:
                        return (false, -1, string.Empty, PreferenceType.WrongLink);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return (false, -1, string.Empty, PreferenceType.WrongLink);
            }

        }

        public void DoSearch()
        {
            var allPrefs = _usersContext.Preferences.Include(u => u.User);
            foreach (var prefs in allPrefs)
            {
                try
                {
                    if (ValidForSearch(prefs))
                    {
                        var keywords = PrepareKeywords(prefs.Keyword);
                        var wallGeParams = new WallGetParams
                        {
                            Count = 50,
                            OwnerId = prefs.TargetType == PreferenceType.VkGroup ? -prefs.TargetId : prefs.TargetId
                        };
                        Thread.Sleep(1000);
                        var getResult = _api.Wall.Get(wallGeParams);
                        var posts = getResult.WallPosts;

                        foreach (var post in posts.Reverse())
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
                catch(UserAuthorizationFailException ex)
                {
                    _logger.Error($"UserAuthorizationException. {ex}");
                    //if (!_api.IsAuthorized)
                    {
                        _logger.Info($"VkApi wasn't authorized. Authorizing..");
                        _api.SimpleAuthorize(_settings.Value.Vkontakte);
                        _logger.Info($"SimpleAuthorize passed. New vk auth status = {_api.IsAuthorized}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error while fetching VK groups. Pref: {prefs.ToShortString()}. Exception message: {ex}");
                }
            }
            _usersContext.SaveChanges();
        }

        bool ValidForSearch(UserPreference prefs)
        {
            return (prefs.TargetId > 0
                && !string.IsNullOrEmpty(prefs.Keyword));
        }
    }

}
