namespace URFX.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<URFX.Data.DataEntity.URFXDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        //protected override void Seed(URFX.Data.DataEntity.URFXDbContext context)
        //{
        //    //  This method will be called after migrating to the latest version.

        //    //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
        //    //  to avoid creating duplicate seed data. E.g.
        //    //
        //    //    context.People.AddOrUpdate(
        //    //      p => p.FullName,
        //    //      new Person { FullName = "Andrew Peters" },
        //    //      new Person { FullName = "Brice Lambson" },
        //    //      new Person { FullName = "Rowan Miller" }
        //    //    );
        //    //

        //    IList<Plan> plans = new List<Plan>();

        //    plans.Add(new Plan()
        //    {
        //        ApplicationFee = 1500,
        //        CreatedDate = DateTime.UtcNow,
        //        Description = "Eco Plan",
        //        Detail = "Means that the Provider is positive about sales also they want a fixed cost on them",
        //        IsActive = true,
        //        PerVisitPercentage = 0,
        //        TeamRegistrationFee = 1500,
        //        TeamRegistrationType = Enums.TeamRegistrationType.Monthly
        //    });
        //    plans.Add(new Plan()
        //    {
        //        ApplicationFee = 1500,
        //        CreatedDate = DateTime.UtcNow,
        //        Description = "Delux Plan",
        //        Detail = "Means that Provider  is less positive on Sales but do not want URFX to charge more if sales incease",
        //        IsActive = true,
        //        PerVisitPercentage = 5,
        //        TeamRegistrationFee = 750,
        //        TeamRegistrationType = Enums.TeamRegistrationType.Monthly
        //    });
        //    plans.Add(new Plan()
        //    {
        //        ApplicationFee = 5000,
        //        CreatedDate = DateTime.UtcNow,
        //        Description = "Premium Plan",
        //        Detail = "Means thatProvider is negative about sales buy they want to test the market with sales",
        //        IsActive = true,
        //        PerVisitPercentage = 10,
        //        TeamRegistrationFee = 500,
        //        TeamRegistrationType = Enums.TeamRegistrationType.Monthly
        //    });            

        //    foreach (Plan plan in plans)
        //    {
        //        context.Plans.Add(plan);
        //    }

        //    base.Seed(context);
        //}
    }
}
