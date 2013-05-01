namespace AngularCRUD.Migrations
{
    using AngularCRUD.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AngularCRUD.Models.TutoringContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AngularCRUD.Models.TutoringContext context)
        {
            var r = new Random();
            var items = Enumerable.Range(1, 50).Select(o => new Tutor
            {
                FullName = "Tutor" + o.ToString(),
                Age = r.Next(10) + 25,
                Certificates = new System.Collections.Generic.List<string>(),
                Sex = r.Next() % 2 == 0
            }).ToArray();
            context.Tutors.AddOrUpdate(item => new { item.FullName }, items);
             

            
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
        }
    }
}
