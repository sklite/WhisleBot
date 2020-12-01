using BotServer.TelegramBot;
using Cyriller;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Text;
using Telegram.Bot;
using VkNet;
using VkNet.Abstractions;
using WhisleBotConsole.BotContorller;
using WhisleBotConsole.Config;
using WhisleBotConsole.DB;
using WhisleBotConsole.TelegramBot;
using WhisleBotConsole.Vk;
using WhisleBotConsole.Vk.Extensions;
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
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var context = ServiceProvider.GetService<UsersContext>();
            context.Database.Migrate();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            Console.OutputEncoding = Encoding.UTF8;


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


            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(tgSettings.AccessT));   
            services.AddSingleton<IBotController, BotController>();
            services.AddSingleton<IVkGroupsCrawler, VkGroupsCrawler>();
            services.AddSingleton<VkGroupsCrawler>();
            services.AddSingleton<ITelegramService, TelegramBotService>();
            services.AddSingleton<ITelegramMessageRouter, TelegramMessageRouter>();
            services.AddSingleton<IPostKeywordSearcher, CaseInsensitiveKeywordSearcher>();
            services.AddSingleton<IMessageSender, TelegramMessageSender>();
            services.AddSingleton<IUserNotifier, UserNewMentionsNotifier>();
            services.AddSingleton<IVkService, VkService>();
            services.AddSingleton<CyrNounCollection, CyrNounCollection>();

            var vkApi = new VkApi();
            vkApi.SimpleAuthorize(vkSettings);
            services.AddSingleton<IVkApi>(vkApi);

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
