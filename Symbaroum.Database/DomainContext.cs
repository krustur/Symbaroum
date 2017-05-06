using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Symbaroum.Database
{
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

    public class DomainContext : DbContext
    {
        //public DomainContext(string nameOrConnectionString)
        //    : base(nameOrConnectionString)
        //{

        //}

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Post>()
        //        .Property(u => u.Content)
        //        .HasColumnName("DaContent");
        //}
    }
}