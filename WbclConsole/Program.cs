using CommandLine;
using System;

namespace WhisleBotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = GetArguments(args);
            var startup = new Startup(arguments);

            var programStarter = new ProgramStarter(startup.ServiceProvider);
            programStarter.Start();
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
