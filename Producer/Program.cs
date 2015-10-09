using Account.Published;
using EasyNetQ;
using Payment.Published;
using System;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = RabbitHutch.CreateBus("host=localhost");

            var amount = new Random();

            var running = true;
            while (running)
            {
                switch (Console.ReadKey(true).KeyChar)
                {
                    case 't':
                        bus.Publish(new TrialStarted("Matthew", "Winder", "mwinder@grr.la", "123 Inc.", DateTime.UtcNow));
                        break;
                    case 's':
                        bus.Publish(new PaymentSucceeded(amount.Next(), DateTime.UtcNow));
                        break;
                    case 'f':
                        bus.Publish(new PaymentFailed(amount.Next(), DateTime.UtcNow));
                        break;
                    case 'q':
                        running = false;
                        break;
                    default:
                        break;
                }
            }

            //var failed = false;
            
            //while (true)
            //{
            //    object message = PublishMessage(bus, amount.Next(), DateTime.UtcNow, failed);
            //    Console.WriteLine($"Published {message}");

            //    failed = !failed;

            //    Thread.Sleep(250);
            //}
        }

        //static object PublishMessage(IBus bus, double amount, DateTime occurredAt, bool failed)
        //{
        //    if (failed)
        //    {
        //        var message = new PaymentFailed(amount, occurredAt);
        //        bus.Publish(message);
        //        return message;
        //    }
        //    else
        //    {
        //        var message = new PaymentSucceeded(amount, occurredAt);
        //        bus.Publish(message);
        //        return message;
        //    }
        //}
    }
}
