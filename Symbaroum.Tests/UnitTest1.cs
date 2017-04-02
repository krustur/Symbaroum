//using System;

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symbaroum.Core.Common.Database;
using System.Data.Entity;
using System.Linq;

namespace Symbaroum.Tests
{
    [TestClass]
    public static class Configuration
    {
        [AssemblyInitialize]
        public static void Configure(TestContext testContext)
        {
            //LocalDbHelper.GetLocalDb("Symbaroum", deleteIfExists: true);
        }
        
    }

    [TestClass]
    public class UnitTest1
    {
        public class BloggingContext : DbContext
        {
            public BloggingContext()
                : base(@"Server=(localdb)\SymbLocalDb;Database=Symbaroum;Integrated Security=True;")
            {
                Database.SetInitializer(new DropCreateDatabaseAlways<BloggingContext>());
            }

            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Post> Posts { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Post>()
                    .Property(u => u.Content)
                    .HasColumnName("da content");
            }
        }

        public class Blog
        {
            public int BlogId { get; set; }
            public string Name { get; set; }

            public virtual List<Post> Posts { get; set; }
        }

        public class Post
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public int BlogId { get; set; }
            public virtual Blog Blog { get; set; }
        }

        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new BloggingContext())
            {
                // Create and save a new Blog 
                //Console.Write("Enter a name for a new Blog: ");
                //var name = Console.ReadLine();
                var name = "Krister";

                var blog = new Blog { Name = name };
                db.Blogs.Add(blog);
                db.SaveChanges();

                // Display all Blogs from the database 
                var query = from b in db.Blogs
                            orderby b.Name
                            select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }

                Console.WriteLine("Press any key to exit...");
                //Console.ReadKey();
            }
        }
    }
}
