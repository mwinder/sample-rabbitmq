using EasyNetQ;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.ElasticSearch;
using Payment.Published;
using System;

namespace Consumer
{
    class Program
    {
        static readonly ILogger log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            ConfigureLogging();

            var bus = RabbitHutch.CreateBus("host=localhost");

            bus.Subscribe<PaymentSucceeded>("Consumer", message => log.Info(message));
            bus.Subscribe<PaymentFailed>("Consumer", message => log.Info(message));

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        private static void ConfigureLogging()
        {
            var logging = new LoggingConfiguration();

            var console = new ColoredConsoleTarget();
            logging.AddTarget("console", console);
            logging.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, console));

            var file = new FileTarget();
            file.FileName = "${basedir}/app.log";
            logging.AddTarget("file", file);
            logging.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, file));

            var elastic = new ElasticSearchTarget();
            logging.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, elastic));

            console.Layout = file.Layout = elastic.Layout = @"${date} [${threadid}] ${logger} ${message}";

            LogManager.Configuration = logging;
        }
    }
}
