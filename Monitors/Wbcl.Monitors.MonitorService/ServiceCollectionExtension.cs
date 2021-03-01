using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.Abstractions;
using Wbcl.Core.Models.Settings;
using Wbcl.Core.Utils;
using Wbcl.Monitors.VkMonitor;
using Wbcl.Monitors.VkMonitor.Posts;
using Wbcl.Monitors.WebMonitor;
using WhisleBotConsole.Vk;

namespace Wbcl.Monitors.MonitorService
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMonitorServices(this IServiceCollection services, Settings settings)
        {

            services.AddSingleton<IVkGroupsCrawler, VkGroupsCrawler>();
            services.AddSingleton<IVkUtils, VkUtils>();
            services.AddSingleton<IPostKeywordSearcher, StupidKeywordSearcher>();

            services.AddSingleton<IUserNotifier, UserNewMentionsNotifier>();
            var vkApi = new VkApi();
            vkApi.SimpleAuthorize(settings.Vkontakte);
            services.AddSingleton<IVkApi>(vkApi);

            services.AddSingleton<IMonitorServiceContainer, MonitorServiceContainer>();
            services.AddSingleton<VkService>();
            services.AddSingleton<WebMonitorService>();

            return services;
        }
    }
}
