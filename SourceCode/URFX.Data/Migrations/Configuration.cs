namespace URFX.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DataEntity;
    using Microsoft.AspNet.Identity;
    using Enums;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<URFX.Data.DataEntity.URFXDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(URFX.Data.DataEntity.URFXDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Roles.AddOrUpdate(
              r => r.Name,
              new ApplicationRole { Name = URFXRoles.Guest.ToString() },
              new ApplicationRole { Name = URFXRoles.Admin.ToString() },
              new ApplicationRole { Name = URFXRoles.Client.ToString() },
              new ApplicationRole { Name = URFXRoles.ServiceProvider.ToString() },
              new ApplicationRole { Name = URFXRoles.Employee.ToString() }

            );

            if (!context.Users.Any(u => u.UserName == DataEntity.Constants.ADMIN_USER_NAME))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = DataEntity.Constants.ADMIN_USER_NAME, Email = DataEntity.Constants.ADMIN_USER_NAME, EmailConfirmed = true };

                manager.Create(user, DataEntity.Constants.ADMIN_USER_PASSWORD);
                manager.AddToRole(user.Id, URFXRoles.Admin.ToString());
            }
            if (!context.Users.Any(u => u.UserName == DataEntity.Constants.GUEST_USER_NAME))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = DataEntity.Constants.GUEST_USER_NAME, Email = DataEntity.Constants.GUEST_USER_NAME, EmailConfirmed = true };

                manager.Create(user, DataEntity.Constants.GUEST_USER_PASSWORD);
                manager.AddToRole(user.Id, URFXRoles.Guest.ToString());
            }
            base.Seed(context);
        }
    }
}
