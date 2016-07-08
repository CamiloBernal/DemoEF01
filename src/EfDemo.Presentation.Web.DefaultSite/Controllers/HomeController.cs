using System.Web.Mvc;
using EfDemo.Presentation.Web.DefaultSite.CodeBase;

namespace EfDemo.Presentation.Web.DefaultSite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index() => View();
    }
}