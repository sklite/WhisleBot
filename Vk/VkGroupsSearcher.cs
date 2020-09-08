using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace BotServer.Vk
{
    public class VkGroupsSearcher
    {
        readonly VkApi _api;
        readonly Logger _logger;

        public VkGroupsSearcher()
        {
            _api = new VkApi();

            _logger = LogManager.GetCurrentClassLogger();

            _api.Authorize(new ApiAuthParams
            {
                ApplicationId = 123456,
                Login = "89167224820",
                Password = "sYZBhzW2",
                Settings = Settings.All
            }) ;
            Console.WriteLine(_api.Token);
        }
     

        public (bool Success, string Result) GetPostLinkByKeyword(string keyword)
        {
			
            var res = _api.Groups.Get(new GroupsGetParams());

   //         Console.WriteLine(res.TotalCount);

   //         Console.WriteLine(res.TotalCount);

			//Console.ReadLine();
            return (true, string.Empty);
		}

        public (bool Success, long GroupId, string GroupName) GetGroupIdByLink(Uri link)
        {
            try
            {
                var groupIdentifier = link.PathAndQuery.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();
                var vkObj = _api.Utils.ResolveScreenName(groupIdentifier);
                if (vkObj == null || vkObj.Type != VkNet.Enums.VkObjectType.Group)
                    return (false, -1, string.Empty);

                var groupInfo = _api.Groups.GetById(null, vkObj.Id.Value.ToString(), GroupsFields.Description);
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
