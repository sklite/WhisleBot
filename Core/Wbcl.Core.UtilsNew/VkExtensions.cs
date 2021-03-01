using VkNet.Abstractions;
using Wbcl.Core.Models.Settings;

namespace Wbcl.Core.Utils
{
    public static class VkExtensions
    {
        public static void SimpleAuthorize(this IVkApi vkApi, VkSettings vkSettings)
        {
            vkApi.Authorize(new VkNet.Model.ApiAuthParams
            {
                ApplicationId = vkSettings.AppId,
                Login = vkSettings.Login,
                Password = vkSettings.Password,
                Settings = VkNet.Enums.Filters.Settings.All
            });
        }
    }
}
