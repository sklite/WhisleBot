using System;
using System.Collections.Generic;
using System.Text;

namespace Wbcl.Monitors.MonitorService
{
    public interface IMonitorServiceContainer
    {
        void StartServices();
        void StopServices();
    }
}
