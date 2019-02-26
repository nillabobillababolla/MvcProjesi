using System.Web.Mvc;
using TeknikServis.Web.App_Code;

namespace TeknikServis.Web.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionHandlerFilterAttribute());
        }
    }
}