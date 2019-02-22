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

            var allTechs = NewRoleManager().FindByName(IdentityRoles.Technician.ToString()).Users.Select(x => x.UserId).ToList();
            var busyTechs = new IssueRepo().GetAll(x => x.IsActive = true).ToList();

            for (int i = 0; i < allTechs.Count; i++)
            {
                var User = NewUserManager().FindById(allTechs[i]);

                foreach (var tech in busyTechs)
                {
                    if (tech.TechnicianId != User.Id)
                    {
                        data.Add(new SelectListItem()
                        {
                            Text = User.Name + " " + User.Surname,
                            Value = User.Id
                        });
                    }
                }
            }
            return data;
        }
    }
}