using EasyNetQ;
using Messages;
using System;
using System.Threading;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = RabbitHutch.CreateBus("host=localhost");

            var amount = new Random();
            var failed = false;

            //Console.WriteLine("Press ENTER to publish type 'exit' to exit");
            //while (Console.ReadLine().ToLower() != "exit")
            while (true)
            {
                object message = PublishMessage(bus, amount.Next(), DateTime.UtcNow, failed);
                Console.WriteLine($"Published {message}");

                failed = !failed;

                Thread.Sleep(250);
            }
        }

        static object PublishMessage(IBus bus, double amount, DateTime occurredAt, bool failed)
        {
            if (failed)
            {
                var message = new PaymentFailed(amount, occurredAt);
                bus.Publish(message);
                return message;
            }
            else
            {
                var message = new PaymentSucceeded(amount, occurredAt);
                bus.Publish(message);
                return message;
            }
        }
    }
}
