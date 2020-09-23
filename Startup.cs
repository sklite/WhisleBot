using BotServer.TelegramBot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.TelegramBot;
using WhisleBotConsole.Vk;
using WhisleBotConsole.Vk.Posts;

namespace WhisleBotConsole
{
    class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        public Startup(Arguments arguments)
        {
            var services = new ServiceCollection();

            Configure();
            ConfigureServices(services, arguments);

            ServiceProvider = services.BuildServiceProvider();

            //var couchbaseAdapter = ServiceProvider.GetService<ICouchbaseAdapter>();
            //couchbaseAdapter.Initialize();
            //couchbaseAdapter.Authenticate();
        }

        private void ConfigureServices(IServiceCollection services, Arguments arguments)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(System.IO.Directory.GetCurrentDirectory())
               .AddJsonFile($"appsettings.json", true, true)
              .Build();

            var tgSettings = config.GetSection("TelegramSettings").Get<TelegramSettings>();
            var vkSettings = config.GetSection("VkSettings").Get<VkSettings>();

            services.Configure<Settings>(settings =>
            {
                settings.Telegram = tgSettings;
                settings.Vkontakte = vkSettings;
            });

            services.AddSingleton<IBotController, BotController>();
            services.AddSingleton<IVkGroupsSearcher, VkGroupsSearcher>();
            services.AddSingleton<VkGroupsSearcher>();
            services.AddSingleton<ITelegramService, TelegramBotService>();
            services.AddSingleton<ITelegramMessageRouter, TelegramMessageRouter>();
            services.AddSingleton<IPostKeywordSearcher, StupidKeywordSearcher>();

            services.AddDbContext<UsersContext>();
            services.AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog();
            });
        }

        private void Configure()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddEnvironmentVariables();

            Configuration = configBuilder.Build();
        }
    }
}
