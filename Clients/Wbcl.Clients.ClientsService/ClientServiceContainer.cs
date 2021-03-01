using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Wbcl.Clients.TelegramClient;
using Wbcl.Core.Models.Services;

namespace Wbcl.Clients.ClientService
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
