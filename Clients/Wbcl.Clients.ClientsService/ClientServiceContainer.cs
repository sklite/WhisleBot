using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Wbcl.Clients.TgClient;
using Wbcl.Core.Models.Services;

namespace Wbcl.Clients.ClientsService
{
    public class ClientServiceContainer : IClientServiceContainer
    {
        List<IClientService> _clients = new List<IClientService>();

        public ClientServiceContainer(IServiceProvider serviceProvider)
        {
            _clients.Add(serviceProvider.GetService<TelegramBotService>());
        }
        public void StartServices()
        {
            foreach (var monitor in _clients)
            {
                monitor.Start();
            }
        }

        public void StopServices()
        {
            foreach (var monitor in _clients)
            {
                monitor.Stop();
            }
        }
    }
}
