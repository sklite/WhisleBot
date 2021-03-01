using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Text;
using Wbcl.Clients.ClientsService;
using Wbcl.Core.Models.Settings;
using Wbcl.DAL;
using Wbcl.Monitors.MonitorService;

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
            ConfigureServices(services, arguments.SettingsFile);

            ServiceProvider = services.BuildServiceProvider();

            //var context = ServiceProvider.GetService<IUsersContext>();
            //context.Database.Migrate();
        }

        private void ConfigureServices(IServiceCollection services, string settingsFileSuffix)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var settingsFile = string.IsNullOrEmpty(settingsFileSuffix) ? "appsettings.json" : $"appsettings.{settingsFileSuffix}.json";
            var appSettings = ReadSettings(settingsFile);
            services//.Configure<Settings>(settings => settings = appSettings);
                .AddSingleton(sp => appSettings);

            services.AddClientServices(appSettings);
            services.AddMonitorServices(appSettings);
            services.AddDatabaseConnector(appSettings.DbSettings.ConnectionString);


            services.AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Warning);
                loggingBuilder.AddNLog();
            });
        }

        private Settings ReadSettings(string settingsFile)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(System.IO.Directory.GetCurrentDirectory())
               .AddJsonFile(settingsFile, true, true)
               .Build();

            var tgSettings = config.GetSection("TelegramSettings").Get<TelegramSettings>();
            var vkSettings = config.GetSection("VkSettings").Get<VkSettings>();
            var dbSettings = config.GetSection("DbSettings").Get<DbSettings>();
            var webSettings = config.GetSection("WebSettings").Get<WebSettings>();

            return new Settings
            {
                Telegram = tgSettings,
                Vkontakte = vkSettings,
                DbSettings = dbSettings,
                WebSettings = webSettings
            };
        }

        private void Configure()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddEnvironmentVariables();

            Configuration = configBuilder.Build();
        }
    }
}
