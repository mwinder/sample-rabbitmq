using EasyNetQ;
using Messages;
using System;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = RabbitHutch.CreateBus("host=localhost");

            bus.Subscribe<PaymentSucceeded>("Consumer", message => Console.WriteLine(message));
            bus.Subscribe<PaymentFailed>("Consumer", message => Console.WriteLine(message));

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
        }
    }
}
