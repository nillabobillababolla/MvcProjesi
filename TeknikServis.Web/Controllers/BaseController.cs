using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeknikServis.BLL.Identity;

namespace TeknikServis.Web.Controllers
{
    [Authorize]
    [RequireHttps]
    public class BaseController : Controller
    {
        protected List<SelectListItem> GetRoleList()
        {
            var data = new List<SelectListItem>();
            MembershipTools.NewRoleStore().Roles
                .ToList()
                .ForEach(x =>
                {
                    data.Add(new SelectListItem()
                    {
                        Text = $"{x.Name}",
                        Value = x.Id
                    });
                });
            return data;
        }
    }
}