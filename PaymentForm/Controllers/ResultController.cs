using System.Web.Mvc;
using PaymentForm.Models;

namespace PaymentForm.Controllers
{
    public class ResultController : Controller
    {
        public ActionResult Fail(FailResultModel failResultModel)
        {
            return View(failResultModel);
        }

        public ActionResult Success(SuccessResultModel successResultModel)
        {
            return View(successResultModel);
        }
    }
}