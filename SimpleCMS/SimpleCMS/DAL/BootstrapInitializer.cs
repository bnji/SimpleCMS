//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Data.Entity;
//using SimpleCMS.Models;
//using System.Data.Entity.Migrations;

//namespace SimpleCMS.DAL
//{
//    //public class BootstrapInitializer : DropCreateDatabaseAlways<BootstrapContext>
//    public class BootstrapInitializer : DbMigrationsConfiguration<BootstrapContext> //MigrateDatabaseToLatestVersion
//    {
//        public BootstrapInitializer()
//        {
//            AutomaticMigrationsEnabled = false;
//            AutomaticMigrationDataLossAllowed = true;
//        }

//        protected override void Seed(BootstrapContext db)
//        {
//            //  This method will be called after migrating to the latest version.
//            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
//            //  to avoid creating duplicate seed data. E.g.
//            //
//            //    context.People.AddOrUpdate(
//            //      p => p.FullName,
//            //      new Person { FullName = "Andrew Peters" },
//            //      new Person { FullName = "Brice Lambson" },
//            //      new Person { FullName = "Rowan Miller" }
//            //    );
//            //
//            CreatePersonTitles(db);
//            CreatePersonGenders(db);
//            db.Banks.AddOrUpdate(new Bank()
//            {
//                RegNumber = 0,
//                Name = "UNKNOWN"
//            });
//            base.Seed(db);
//            db.SaveChanges();
//        }

//        private void CreatePersonGenders(BootstrapContext db)
//        {
//            db.PersonGenders.AddOrUpdate(new PersonGender()
//            {
//                Type = "M"
//            });
//            db.PersonGenders.AddOrUpdate(new PersonGender()
//            {
//                Type = "F"
//            });
//            db.PersonGenders.AddOrUpdate(new PersonGender()
//            {
//                Type = "Unknown"
//            });
//        }

//        private void CreatePersonTitles(BootstrapContext db)
//        {
//            db.PersonTitles.AddOrUpdate(new PersonTitle()
//            {
//                Name = "Mr."
//            });
//            db.PersonTitles.AddOrUpdate(new PersonTitle()
//            {
//                Name = "Mrs."
//            });
//            db.PersonTitles.AddOrUpdate(new PersonTitle()
//            {
//                Name = "Unknown"
//            });
//        }
//    }
//}
