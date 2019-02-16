using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using TeknikServis.Models.Entities;
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

        public virtual DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Issue>()
                .Property(x => x.ServiceCharge)
                .HasPrecision(6,2);
        }
    }
}
