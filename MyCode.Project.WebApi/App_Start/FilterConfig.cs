using System.Web;
using System.Web.Mvc;
using MyCode.Project.WebApi.App_Filter;

namespace MyCode.Project.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            
            filters.Add(new HandleErrorAttribute());

        }
    }
}
