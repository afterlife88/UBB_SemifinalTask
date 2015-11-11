using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using UBB_SemifinalTask.Data;
using UBB_SemifinalTask.Models;

namespace UBB_SemifinalTask.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<UBB_SemifinalTask.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(UBB_SemifinalTask.Data.ApplicationDbContext context)
        {
       //context.Database.Delete();
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            //var user = new ApplicationUser()
            //{
            //    Email = "extragalactic88@gmail.com",
            //    EmailConfirmed = true,
            //    Category = "Fiz lico",
            //    Name = "Afterlife",
            //    UserName = "somename"

            //};

            //manager.Create(user, "19951304");
        }
    }
}
