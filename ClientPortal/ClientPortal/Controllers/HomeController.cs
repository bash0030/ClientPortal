using System.Web.Mvc;

namespace ClientPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TempData.Clear();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Aliases()
        {
            ViewBag.Message = "Your aliases page.";

            return View();
        }
    }
}