using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using Autofac;
using Symbaroum.Core.Infrastructure;

namespace Symbaroum.Core.Common.Database
{
    public interface IDomainContextFactory
    {
        DomainContext Create();
    }

    public class DomainContextFactory : IDomainContextFactory
    {
        private readonly IConfigReader _configReader;

        public DomainContextFactory(IConfigReader configReader)
        {
            _configReader = configReader;
        }


        public DomainContext Create()
        {
            return new DomainContext(_configReader.ConnectionString);
        }
    }


    public class DesignTimeDomainContextFactory : IDbContextFactory<DomainContext>
    {
        public DomainContext Create()
        {
            var contextFactory = Bootstrap.Container.Resolve<IDomainContextFactory>();
            return contextFactory.Create();
        }
    }

    public class DomainContextMigrationsConfiguration : DbMigrationsConfiguration<DomainContext>
    {
        public DomainContextMigrationsConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
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

    public class DomainContext : DbContext
    {
        public DomainContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(u => u.Content)
                .HasColumnName("DaContent");
        }
    }
}
