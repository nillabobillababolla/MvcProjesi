using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using TeknikServis.DAL;
using TeknikServis.Models.IdentityModels;

namespace TeknikServis.BLL.Identity
{
    public static class MembershipTools
    {
        private static readonly MyContext _db;

        public static UserStore<User> NewUserStore() => new UserStore<User>(_db ?? new MyContext());
        public static UserManager<User> NewUserManager() => new UserManager<User>(NewUserStore());

        public static RoleStore<Role> NewRoleStore() => new RoleStore<Role>(_db ?? new MyContext());
        public static RoleManager<Role> NewRoleManager() => new RoleManager<Role>(NewRoleStore());


        public static string GetNameSurname(string userId)
        {
            User user;
            if (string.IsNullOrEmpty(userId))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                {
                    return "";
                }

                user = NewUserManager().FindById(id);
            }
            else
            {
                user = NewUserManager().FindById(userId);
                if (user == null)
                {
                    return null;
                }
            }

            return $"{user.Name} {user.Surname}";
        }

        public static string GetNameSurnameCurrent()
        {
            User user;
            var id = HttpContext.Current.User.Identity.GetUserId();

            user = NewUserManager().FindById(id);
            return $"{user.Name} {user.Surname}";
        }

        public static string GetEmailCurrent()
        {
            User user;
            var id = HttpContext.Current.User.Identity.GetUserId();

            user = NewUserManager().FindById(id);
            return $"{user.Email}";
        }

    public static string GetAvatarPath(string userId)
    {
        User user;
        if (string.IsNullOrEmpty(userId))
        {
            var id = HttpContext.Current.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(id))
            {
                return "/assets/images/icon-noprofile.png";
            }

            user = NewUserManager().FindById(id);
        }
        else
        {
            user = NewUserManager().FindById(userId);
            if (user == null)
            {
                return "/assets/images/icon-noprofile.png";
            }
        }

        return $"{user.AvatarPath}";
    }
}
}