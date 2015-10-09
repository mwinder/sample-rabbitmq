using Account.Published;
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
            LogManager.Configuration = LoggingConfiguration();

            var bus = RabbitHutch.CreateBus("host=localhost");

            bus.Subscribe<PaymentSucceeded>("Consumer", message => log.Info(message));
            bus.Subscribe<PaymentFailed>("Consumer", message => log.Info(message));

            var trialStartedHandler = new TrialStartedHandler();
            bus.Subscribe<TrialStarted>("Consumer", trialStartedHandler.Handle);

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }

        private static LoggingConfiguration LoggingConfiguration()
        {
            var configuration = new LoggingConfiguration();

            var console = new ColoredConsoleTarget();
            configuration.AddTarget("console", console);
            configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, console));

            var file = new FileTarget();
            file.FileName = "${basedir}/app.log";
            configuration.AddTarget("file", file);
            configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, file));

            var elastic = new ElasticSearchTarget();
            configuration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, elastic));

            console.Layout = file.Layout = elastic.Layout = @"${date} [${threadid}] ${logger} ${message}";

            return configuration;
        }
    }

    class TrialStartedHandler
    {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public void Handle(TrialStarted trialStarted)
        {
            Log.Info("Handling trial started");
        }
    }
}
