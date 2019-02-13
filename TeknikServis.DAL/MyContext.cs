using Microsoft.AspNet.Identity.EntityFramework;
using System;
using TeknikServis.Models.IdentityModels;

namespace TeknikServis.DAL
{
    public class MyContext : IdentityDbContext<User>
    {
        public DateTime InstanceDate { get; set; }

        public MyContext() : base("MyCon")
        {
            InstanceDate = DateTime.Now;
        }

    }
}
