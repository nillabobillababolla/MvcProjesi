using System;
using System.Collections.Generic;
using System.Linq;
using TeknikServis.BLL.Identity;
using TeknikServis.Models.IdentityModels;

namespace TeknikServis.BLL.Helpers
{
    public static class MockDataHelpers
    {
        public static async void AddMockUsersAsync()
        {
            var usermanager = MembershipTools.NewUserManager();
            var userstore = MembershipTools.NewUserStore();

            List<User> Users = new List<User>();

            var adm = new User()
            {
                Email = "admin@gmail.com",
                AvatarPath = "/assets/images/icon-noprofile.png",
                EmailConfirmed = true,
                UserName = "admin",
                Name = "Admin",
                PhoneNumber = "123456789",
                Surname = "adminsurname",
                RegisterDate = DateTime.Now,
                PhoneNumberConfirmed = true,
            };
            Users.Add(adm);
            var opr = new User()
            {
                Email = "operator@gmail.com",
                AvatarPath = "/assets/images/icon-noprofile.png",
                EmailConfirmed = true,
                UserName = "operator",
                Name = "Operator",
                PhoneNumber = "123456789",
                Surname = "operatorsurname",
                RegisterDate = DateTime.Now,
                PhoneNumberConfirmed = true,
            };
            Users.Add(opr);
            var tech = new User()
            {
                Email = "technician@gmail.com",
                AvatarPath = "/assets/images/icon-noprofile.png",
                EmailConfirmed = true,
                UserName = "technician",
                Name = "Technician",
                PhoneNumber = "123456789",
                Surname = "techniciansurname",
                RegisterDate = DateTime.Now,
                PhoneNumberConfirmed = true,
            };
            Users.Add(tech);
            var cust = new User()
            {
                Email = "customer@gmail.com",
                AvatarPath = "/assets/images/icon-noprofile.png",
                EmailConfirmed = true,
                UserName = "customer",
                Name = "Customer",
                PhoneNumber = "123456789",
                Surname = "customersurname",
                RegisterDate = DateTime.Now,
                PhoneNumberConfirmed = true,
            };
            Users.Add(cust);

            foreach (var user in Users)
            {
                var newPassword = "123456";
                var result = await usermanager.CreateAsync(user, newPassword);

                if (result.Succeeded)
                {
                    switch (userstore.Users.Count())
                    {
                        case 1:
                            await usermanager.AddToRoleAsync(user.Id, "Admin");
                            break;
                        case 2:
                            await usermanager.AddToRoleAsync(user.Id, "Operator");
                            break;
                        case 3:
                            await usermanager.AddToRoleAsync(user.Id, "Technician");
                            break;
                        default:
                            await usermanager.AddToRoleAsync(user.Id, "Customer");
                            break;
                    }
                }
            }
        }
    }
}
