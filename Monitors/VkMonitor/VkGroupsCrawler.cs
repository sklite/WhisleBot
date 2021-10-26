using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using VkNet.Abstractions;
using VkNet.Enums;
using VkNet.Exception;
using VkNet.Model.RequestParams;
using Wbcl.Core.Models.Database;
using Wbcl.Core.Models.Settings;
using Wbcl.Core.Utils;
using Wbcl.DAL.Context;
using Wbcl.Monitors.VkMonitor.Posts;

namespace Wbcl.Monitors.VkMonitor
{
    public class VkGroupsCrawler : IVkGroupsCrawler
    {
        private readonly IVkApi _api;
        private readonly Settings _settings;
        private readonly Logger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPostKeywordSearcher _keywordSearcher;
        private readonly IUserNotifier _userNotifier;
        private readonly List<VkObjectType> _supportedVkTypes;

        public VkGroupsCrawler(
            IServiceProvider serviceProvider,
            IPostKeywordSearcher keywordSearcher,
            IUserNotifier userNotifier,
            IVkApi vkApi,
            Settings settings)
        {
            _api = vkApi;
            _settings = settings;
            _logger = LogManager.GetCurrentClassLogger();
            _serviceProvider = serviceProvider;
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

        public void DoSearch()
        {
            using (var usersContext = _serviceProvider.GetRequiredService<IUsersContext>())
            {
                var allPrefs = usersContext.Preferences.Include(u => u.User);
                foreach (var prefs in allPrefs)
                {
                    try
                    {
                        if (!ValidForSearch(prefs)) 
                            continue;

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
                    catch (UserAuthorizationFailException ex)
                    {
                        _logger.Error($"UserAuthorizationException. {ex}");
                        //if (!_api.IsAuthorized)
                        {
                            _logger.Info($"VkApi wasn't authorized. Authorizing..");
                            _api.SimpleAuthorize(_settings.Vkontakte);
                            _logger.Info($"SimpleAuthorize passed. New vk auth status = {_api.IsAuthorized}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"Error while fetching VK groups. Pref: {prefs.ToShortString()}. Exception message: {ex}");
                    }
                }
                usersContext.SaveChanges();
            }
        }

        bool ValidForSearch(UserPreference prefs)
        {
            return (prefs.TargetId > 0
                && !string.IsNullOrEmpty(prefs.Keyword));
        }
    }

}
