using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VkNet;
using VkNet.Abstractions;
using WhisleBotConsole.Config;

namespace WhisleBotConsole.Vk.Extensions
{
    internal static class VkExtensions
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
