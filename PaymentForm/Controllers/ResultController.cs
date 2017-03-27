using System.Web.Mvc;
using PaymentForm.Models;

namespace PaymentForm.Controllers
{
    public class ResultController : Controller
    {
        public ActionResult Fail(FailResultModel failResultModel)
        {
            if (failResultModel.Message == null)
            {
                failResultModel.Message = "При обработке платежа произошла ошибка, попробуйте повторить оплату позже";
            }

            return View(failResultModel);
        }

        public ActionResult Success(SuccessResultModel successResultModel)
        {
            return View(successResultModel);
        }
    }
}