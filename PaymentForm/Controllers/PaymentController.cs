using System.Web.Mvc;
using PaymentForm.Domain;
using PaymentForm.Models;

namespace PaymentForm.Controllers
{
    public abstract class PaymentController : Controller
    {
        protected ActionResult ProcessPaymentResponse(PaymentResponse paymentResponse)
        {
            if (paymentResponse.IsSuccess)
            {
                return RedirectToAction("Success", "Result",
                    new SuccessResultModel
                    {
                        OrderId = paymentResponse.OrderId,
                        Amount = paymentResponse.Amount,
                        Currency = paymentResponse.Currency,
                        CreatedDate = paymentResponse.CreatedDate,
                        TransactionId = paymentResponse.TransactionId
                    });
            }

            if (paymentResponse.ThreeDSecure != null)
            {
                return RedirectToAction("Index", "ThreeDSecure",
                    new ThreeDSecureModel
                    {
                        MD = paymentResponse.TransactionId.ToString(),
                        PaReq = paymentResponse.ThreeDSecure.PaReq,
                        AcsUrl = paymentResponse.ThreeDSecure.AcsUrl
                    });
            }

            return RedirectToAction("Fail", "Result",
                new FailResultModel
                {
                    Message =
                        paymentResponse.Message ??
                        "При обработке платежа произошла ошибка, попробуйте повторить оплату позже"
                });
        }
    }
}