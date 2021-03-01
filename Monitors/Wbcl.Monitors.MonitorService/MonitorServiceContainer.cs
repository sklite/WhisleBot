using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Wbcl.Core.Models.Services;
using Wbcl.Monitors.WebMonitor;
using WhisleBotConsole.Vk;

namespace Wbcl.Monitors.MonitorService
{
    public class MonitorServiceContainer : IMonitorServiceContainer
    {
        List<IMonitorService> _monitors = new List<IMonitorService>();
 
        public MonitorServiceContainer(IServiceProvider serviceProvider)
        {
            _monitors.Add(serviceProvider.GetService<VkService>());
            _monitors.Add(serviceProvider.GetService<WebMonitorService>());
        }

        public void AddMonitors(ICollection<IMonitorService> monitors)
        {
            foreach (var monitor in monitors)
            {
                _monitors.Add(monitor);
            }
        }

        public void StartServices()
        {
            foreach (var monitor in _monitors)
            {
                monitor.Start();
            }
        }

        public void StopServices()
        {
            foreach (var monitor in _monitors)
            {
                monitor.Stop();
            }
        }
    }
}
