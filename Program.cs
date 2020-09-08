using CommandLine;
using NLog;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace WhisleBotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                var arguments = GetArguments(args);
                var startup = new Startup(arguments);

                var service = startup.ServiceProvider.GetService<IBotController>();
                service.Start();

                Console.WriteLine("Press ANY key to exit");
                Console.ReadKey();
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

        private static Arguments GetArguments(string[] args)
        {
            Arguments arguments = null;

            Parser.Default.ParseArguments<Arguments>(args)
                .WithParsed((p) => arguments = p)
                .WithNotParsed(errors => throw new ArgumentException(string.Join(", ", errors)));

            return arguments;
        }
    }
}
