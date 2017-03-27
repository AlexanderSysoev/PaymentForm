namespace PaymentForm.Models
{
    public class PaymentDetailsModel
    {
        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string CardHolderName { get; set; }

        public string CardCryptogramPacket { get; set; }
    }
}