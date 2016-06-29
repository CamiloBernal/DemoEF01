using System.Web.Mvc;
using EfDemo.Data.Presentation.Web.DefaultSite.CodeBase;

namespace EfDemo.Data.Presentation.Web.DefaultSite.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index() => View();
    }
}