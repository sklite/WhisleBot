using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Threading;
using Wbcl.Clients.ClientService;
using Wbcl.Monitors.MonitorService;

namespace WhisleBotConsole
{
    class ProgramStarter
    {
        private IMonitorServiceContainer _monitors;
        private IClientServiceContainer _clients;


        public ProgramStarter(IServiceProvider serviceProvider)
        {
            _monitors = serviceProvider.GetService<IMonitorServiceContainer>();
            _clients = serviceProvider.GetService<IClientServiceContainer>();
        }

        public void Start()
        {
            var logger = LogManager.GetCurrentClassLogger();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            try
            {
                _clients.StartServices();
                _monitors.StartServices();

                Console.WriteLine("Press ANY key to exit");
                new AutoResetEvent(false).WaitOne();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            _clients?.StopServices();
            _monitors?.StopServices();
        }

        private Arguments GetArguments(string[] args)
        {
            Arguments arguments = null;

            Parser.Default.ParseArguments<Arguments>(args)
                .WithParsed((p) => arguments = p)
                .WithNotParsed(errors => throw new ArgumentException(string.Join(", ", errors)));

            return arguments;
        }
    }
}
