using System;

namespace Messages
{
    public class PaymentFailed
    {
        public readonly double amount;
        public readonly DateTime occurredAt;

        public PaymentFailed(double amount, DateTime occurredAt)
        {
            this.amount = amount;
            this.occurredAt = occurredAt;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {amount} at {occurredAt}";
        }
    }
}
