using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VkNet.Abstractions;
using VkNet.Enums;
using Wbcl.Core.Models.Database;

namespace Wbcl.Core.Utils
{
    public class VkUtils : IVkUtils
    {
        private readonly IVkApi _api;
        private readonly Logger _logger;
        private readonly List<VkObjectType> _supportedVkTypes;

        public VkUtils(IVkApi vkApi)
        {
            _api = vkApi;
            _logger = LogManager.GetCurrentClassLogger();
            _supportedVkTypes = new List<VkObjectType> { VkObjectType.Group, VkObjectType.User };
        }

        public (bool Success, long Id, string Name, PreferenceType LinkType) GetObjIdIdByLink(Uri link)
        {
            try
            {
                var groupIdentifier = link.LocalPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
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
    }
}
