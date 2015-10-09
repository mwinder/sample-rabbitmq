using System;

namespace Payment.Published
{
    public class PaymentSucceeded
    {
        public PaymentSucceeded(double amount, DateTime occurredAt)
        {
            Amount = amount;
            OccurredAt = occurredAt;
        }

        public double Amount { get; private set; }

        public DateTime OccurredAt { get; private set; }

        public override string ToString()
        {
            return $"{GetType().Name}: {Amount} at {OccurredAt}";
        }
    }
}
