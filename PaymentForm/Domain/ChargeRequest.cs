namespace PaymentForm.Domain
{
    public class ChargeRequest
    {
        public int InvoiceId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string IpAddress { get; set; }

        public string Name { get; set; }

        public string CardCryptogramPacket { get; set; }
    }
}