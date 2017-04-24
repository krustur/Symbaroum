using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Transactions;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symbaroum.Core.Common;
using Symbaroum.Core.Common.Database;
using Symbaroum.Core.Infrastructure;

namespace Symbaroum.Tests
{
    [TestClass]
    public class IntegrationTestsBase
    {
        private TransactionScope _transactionScope;

        [TestInitialize]
        public void Initialize()
        {
            _transactionScope = new TransactionScope();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Transaction.Current.Rollback();
            _transactionScope.Dispose();
        }
    }

    [TestClass]
    public class IntegrationTest
    {
        private string _connectionString;

        [TestInitialize]
        public void Arrange()
        {
            var configReader = Bootstrap.Container.Resolve<IConfigReader>();
            _connectionString = configReader.ConnectionString;
        }

        // you don't want any of these executed automatically
        [TestMethod]
        //[Ignore] // Only for manual execution
        public void Wipe_And_Create_Database()
        {
            // drop database first
            LocalDbHelper.DropDatabase(_connectionString);
            var domainContextFactory = Bootstrap.Container.Resolve<IDomainContextFactory>();
            using (var domainContext = domainContextFactory.Create())
            {
                LocalDbHelper.ForceInitialization(domainContext);
            }
           
            // And after the DB is created, you can put some initial base data 
            // for your tests to use
            // usually this data represents lookup tables, like Currencies, Countries, Units of Measure, etc
            using (var domainContext = domainContextFactory.Create())
            {
                Console.WriteLine("Seeding test data into database");
                // discussion for that to follow
                
                //SeedContextForTests.Seed(domainContext);

                Console.WriteLine("Seeding test data is complete");
            }
        }

        

        // this method is only updates your DB to latest migration.
        // does the same as if you run "Update-Database" in nuget console in Visual Studio
        [TestMethod]
        [Ignore] // Only for manual execution
        public void Update_Database()
        {           
            var migrationConfiguration = new DomainContextMigrationsConfiguration();

            migrationConfiguration.TargetDatabase = new DbConnectionInfo(_connectionString, "System.Data.SqlClient");

            var migrator = new DbMigrator(migrationConfiguration);

            migrator.Update();
        }

        [TestMethod]
        public void TestMethod1()
        {
            //using (var db = new DomainContext())
            //{
            //    // Create and save a new Blog 
            //    var name = "Krister";

            //    var blog = new Blog { Name = name };
            //    db.Blogs.Add(blog);
            //    db.SaveChanges();

            //    // Display all Blogs from the database 
            //    var query = from b in db.Blogs
            //                orderby b.Name
            //                select b;

            //    Console.WriteLine("All blogs in the database:");
            //    foreach (var item in query)
            //    {
            //        Console.WriteLine(item.Name);
            //    }

            //    Console.WriteLine("Press any key to exit...");
            //}
        }
    }
}