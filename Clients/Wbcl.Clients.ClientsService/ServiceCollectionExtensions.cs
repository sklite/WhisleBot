using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Wbcl.Clients.TgClient;
using Wbcl.Clients.TgClient.Messaging;
using Wbcl.Core.Models.Services;
using Wbcl.Core.Models.Settings;

namespace Wbcl.Clients.ClientsService
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services, Settings settings)
        {
            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(settings.Telegram.AccessT));
            services.AddSingleton<ITelegramMessageRouter, TelegramMessageRouter>();
            services.AddSingleton<IMessageSender, TelegramMessageSender>();


            services.AddSingleton<IClientServiceContainer, ClientServiceContainer>();
            services.AddSingleton<TelegramBotService>();
            

            return services;
        }
    }
}
