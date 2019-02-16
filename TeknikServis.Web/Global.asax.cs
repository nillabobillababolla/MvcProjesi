using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using TeknikServis.BLL.Helpers;
using TeknikServis.BLL.Identity;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Enums;
using TeknikServis.Models.IdentityModels;
using TeknikServis.Web.App_Start;

namespace TeknikServis.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoMapperConfig.RegisterMappings();
            var roller = Enum.GetNames(typeof(IdentityRoles));
            var userManager = MembershipTools.NewUserManager();
            var userStore = MembershipTools.NewUserStore();
            var roleManager = MembershipTools.NewRoleManager();
            foreach (var rol in roller)
            {
                if (!roleManager.RoleExists(rol))
                    roleManager.Create(new Role()
                    {
                        Name = rol
                    });
            }

            if (!userStore.Users.Any())
            {
                MockDataHelpers.AddMockUsersAsync();
            }

            if (new ProductRepo().GetAll().Count == 0)
            {
                MockDataHelpers.AddMockProductsAsync();
            }
        }
    }
}