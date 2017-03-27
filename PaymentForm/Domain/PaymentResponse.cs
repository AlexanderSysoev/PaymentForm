using System;

namespace PaymentForm.Domain
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public int TransactionId { get; set; }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime CreatedDate { get; set; }

        public ThreeDSecure ThreeDSecure { get; set; }
    }
}