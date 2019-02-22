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

            for (int i = 0; i < 3; i++)
            {
                var adm = new User()
                {
                    Email = $"admin{i+1}@gmail.com",
                    UserName = $"admin{i+1}",
                    Name = $"Admin{i+1}",
                    Surname = $"adminsurname{i+1}",
                    Location = Models.Enums.Locations.Beşiktaş,
                };
                Users.Add(adm);
                var opr = new User()
                {
                    Email = $"operator{i+1}@gmail.com",
                    UserName = $"operator{i+1}",
                    Name = $"Operator{i+1}",
                    Location = Models.Enums.Locations.Esenler,
                    Surname = $"operatorsurname{i+1}",
                };
                Users.Add(opr);
                var tech = new User()
                {
                    Email = $"technician{i+1}@gmail.com",
                    UserName = $"technician{i+1}",
                    Name = $"Technician{i+1}",
                    Location = Models.Enums.Locations.Kağıthane,
                    Surname = $"techniciansurname{i+1}",
                };
                Users.Add(tech);
                var cust = new User()
                {
                    Email = $"customer{i+1}@gmail.com",
                    UserName = $"customer{i+1}",
                    Name = $"Customer{i+1}",
                    Location = Models.Enums.Locations.Kadıköy,
                    Surname = $"customersurname{i+1}",
                };
                Users.Add(cust);
            }

            foreach (var user in Users)
            {
                var newPassword = "123456";
                var result = await usermanager.CreateAsync(user, newPassword);
                user.AvatarPath = "/assets/images/icon-noprofile.png";
                user.EmailConfirmed = true;
                user.RegisterDate = DateTime.Now;
                user.PhoneNumber = "123456789";
                user.PhoneNumberConfirmed = true;

                if (result.Succeeded)
                {
                    switch (userstore.Users.Count())
                    {
                        case 1:
                        case 5:
                        case 9:
                            await usermanager.AddToRoleAsync(user.Id, "Admin");
                            break;
                        case 2:
                        case 6:
                        case 10:
                            await usermanager.AddToRoleAsync(user.Id, "Operator");
                            break;
                        case 3:
                        case 7:
                        case 11:
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
