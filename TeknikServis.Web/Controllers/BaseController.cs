using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TeknikServis.BLL.Identity;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Enums;
using static TeknikServis.BLL.Identity.MembershipTools;

namespace TeknikServis.Web.Controllers
{
    [Authorize]
    // [RequireHttps]
    public class BaseController : Controller
    {
        protected List<SelectListItem> GetRoleList()
        {
            var data = new List<SelectListItem>();
            NewRoleStore().Roles
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

        protected List<SelectListItem> GetTechnicianList()
        {
            var data = new List<SelectListItem>();
            var userManager = NewUserManager();
            var users = userManager.Users.ToList();

            var techIds = new IssueRepo().GetAll(x => x.IssueState == IssueStates.İşlemde || x.IssueState == IssueStates.Atandı).Select(x => x.TechnicianId).ToList();

            foreach (var user in users)
            {
                if (userManager.IsInRole(user.Id, IdentityRoles.Technician.ToString()))
                {
                    if (!techIds.Contains(user.Id))
                    {
                        data.Add(new SelectListItem()
                        {
                            Text = $"{user.Name} {user.Surname} ({GetTechPoint(user.Id)})",
                            Value = user.Id
                        });
                    }
                }
            }
            return data;
        }
    }
}