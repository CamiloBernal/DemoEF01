using System.Web.Mvc;

namespace EfDemo.Data.Presentation.Web.DefaultSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}