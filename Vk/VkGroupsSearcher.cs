using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Linq;
using System.Threading;
using System.Timers;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using WhisleBotConsole.BotContorller;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.Vk.Posts;

namespace WhisleBotConsole.Vk
{
    class VkGroupsSearcher : IVkGroupsSearcher
    {
        private readonly VkApi _api;        
        private readonly Logger _logger;
        private readonly UsersContext _usersContext;
        private readonly IPostKeywordSearcher _keywordSearcher;
        private readonly IUserNotifier _userNotifier;
        private readonly Settings _settings;
        private bool IsSearching = false;
        System.Timers.Timer _timer;


        public VkGroupsSearcher(UsersContext usersContect,
            IPostKeywordSearcher keywordSearcher,
            IUserNotifier userNotifier,
            IOptions<Settings> settings)
        {
            _settings = settings.Value;
            _api = new VkApi();

            _logger = LogManager.GetCurrentClassLogger();

            _api.Authorize(new ApiAuthParams
            {
                ApplicationId = _settings.Vkontakte.AppId,
                Login = _settings.Vkontakte.Login,
                Password = _settings.Vkontakte.Password,
                Settings = VkNet.Enums.Filters.Settings.All
            }) ;
            Console.WriteLine(_api.Token);
            _usersContext = usersContect;
            _keywordSearcher = keywordSearcher;
            _userNotifier = userNotifier;
        }
        private void SetTimer(int milliseconds)
        {
            // Create a timer with a two second interval.
            _timer = new System.Timers.Timer(milliseconds);
            // Hook up the Elapsed event for the timer. 
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private string[] PrepareKeywords(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return null;
            var keywords = keyword.ToLower().Split(',');
            return keywords;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            _logger.Info("Searching mentions...");
            //Avoid multiple vk calls at the same time
            if (IsSearching)
                return;

            IsSearching = true;
            var allPrefs = _usersContext.Preferences.Include(u => u.User);
            foreach (var prefs in allPrefs)
            {
                if (prefs.GroupId > 0 && !string.IsNullOrEmpty(prefs.Keyword))
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

                        _userNotifier.NotifyUser(prefs.User.Id, prefs.GroupId, post.Id.Value, searchResult.Word);
                    }

                }
            }
            IsSearching = false;
        }

        public void StartSearch(int interval)
        {
            SetTimer(interval);
            _timer.Start();   
        }

        public void StopSearch()
        {
            _timer.Stop();
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
    }

}
