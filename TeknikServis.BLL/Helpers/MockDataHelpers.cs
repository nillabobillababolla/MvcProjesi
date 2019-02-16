using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using TeknikServis.BLL.Identity;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;
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
                await userstore.SetPasswordHashAsync(user, usermanager.PasswordHasher.HashPassword(newPassword));
                await usermanager.CreateAsync(user);
            }
        }

        public static async void AddMockProductsAsync()
        {
            List<Product> products = new List<Product>();

            var prod1 = new Product
            {
                ProductName = "Bosch Süpürge",
            };
            products.Add(prod1);
            var prod2 = new Product
            {
                ProductName = "Bosch Fırın",
            };
            products.Add(prod2);
            var prod3 = new Product
            {
                ProductName = "Siemens Buzdolabı",
            };
            products.Add(prod3);
            var prod4 = new Product
            {
                ProductName = "Siemens Bulaşık Makinesi",
            };
            products.Add(prod4);
            var prod5 = new Product
            {
                ProductName = "Siemens Çamaşır Makinesi",
            };
            products.Add(prod5);

            foreach (var product in products)
            {
                await new ProductRepo().InsertAsync(product);
            }
        }
    }
}
