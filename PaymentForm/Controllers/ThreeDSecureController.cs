using System.Threading.Tasks;
using System.Web.Mvc;
using PaymentForm.Models;
using PaymentForm.Services;

namespace PaymentForm.Controllers
{
    public class ThreeDSecureController : PaymentController
    {
        private readonly PaymentService _paymentService;

        public ThreeDSecureController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public ActionResult Index(ThreeDSecureModel threeDSecureModel)
        {
            return View(threeDSecureModel);
        }

        [HttpPost]
        public async Task<ActionResult> Post3DS(int MD, string PaRes)
        {
            var paymentResponse = await _paymentService.PostThreeDSAsync(MD, PaRes);
            return ProcessPaymentResponse(paymentResponse);
        }
    }
}