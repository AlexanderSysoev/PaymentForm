using System.Threading.Tasks;
using System.Web.Mvc;
using PaymentForm.Domain;
using PaymentForm.Models;
using PaymentForm.Services;

namespace PaymentForm.Controllers
{
    public class ChargeController : PaymentController
    {
        private readonly PaymentService _paymentService;

        public ChargeController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var orderInfo = new OrderInfo
            {
                OrderId = 123,
                Amount = 100,
                Currency = "RUB"
            };

            Session[orderInfo.OrderId.ToString()] = orderInfo;

            return View(CreatePaymentDetailsModel(orderInfo));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(PaymentDetailsModel paymentDetailsModel)
        {
            var orderInfo = Session[paymentDetailsModel.OrderId.ToString()] as OrderInfo;
            if (orderInfo == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                var paymentResponse = await _paymentService.ChargeAsync(new ChargeRequest
                {
                    InvoiceId = paymentDetailsModel.OrderId,
                    Amount = orderInfo.Amount,
                    Currency = orderInfo.Currency,
                    IpAddress = GetIpAddress(),
                    Name = paymentDetailsModel.CardHolderName,
                    CardCryptogramPacket = paymentDetailsModel.CardCryptogramPacket
                });

                return ProcessPaymentResponse(paymentResponse);
            }

            return View(CreatePaymentDetailsModel(orderInfo));
        }

        private PaymentDetailsModel CreatePaymentDetailsModel(OrderInfo orderInfo)
        {
            return new PaymentDetailsModel
            {
                OrderId = orderInfo.OrderId,
                Amount = orderInfo.Amount,
                Currency = orderInfo.Currency
            };
        }

        private string GetIpAddress()
        {
            if (Request.Headers["CF-CONNECTING-IP"] != null)
            {
                return Request.Headers["CF-CONNECTING-IP"];
            }

            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                return Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            return Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress;
        }
    }
}