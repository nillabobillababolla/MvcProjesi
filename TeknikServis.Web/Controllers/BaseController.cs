using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
            var users = NewUserManager().Users.ToList();

            foreach (var user in users)
            {
                if (NewUserManager().IsInRole(user.Id, IdentityRoles.Technician.ToString()))
                {
                    var tech = new IssueRepo().GetAll().FirstOrDefault(issue => issue.TechnicianId == user.Id && issue.IssueState!=IssueStates.İşlemde);
                    if (tech == null)
                    {
                        data.Add(new SelectListItem()
                        {
                            Text = $"{user.Name} {user.Surname}",
                            Value = user.Id
                        });
                    }
                }
            }
            return data;
        }
    }
}