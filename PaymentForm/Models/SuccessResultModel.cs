using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentForm.Models
{
    public class SuccessResultModel
    {
        [Display(Name = "Номер заказа")]
        public int OrderId { get; set; }

        [Display(Name = "Сумма")]
        public decimal Amount { get; set; }

        [Display(Name = "Валюта")]
        public string Currency { get; set; }

        [Display(Name = "Дата платежа")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Номер транзакции")]
        public int TransactionId { get; set; }
    }
}