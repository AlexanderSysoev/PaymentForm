namespace PaymentForm.Domain
{
    public class OrderInfo
    {
        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }
}