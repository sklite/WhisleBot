﻿using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Wbcl.Clients.ClientService;
using Wbcl.Clients.TelegramClient;
using Wbcl.Clients.TgClient;
using Wbcl.Core.Models.Services;
using Wbcl.Core.Models.Settings;
using WhisleBotConsole.TelegramBot;

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
