using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using Autofac;

namespace Symbaroum.Infrastructure.Database
{
    //public class DesignTimeDomainContextFactory : IDbContextFactory<DomainContext>
    //{
    //    public DomainContext Create()
    //    {
    //        var contextFactory = Bootstrap.Bootstrap.Container.Resolve<IDomainContextFactory>();
    //        return contextFactory.Create();
    //    }
    //}

    //public class DomainContextMigrationsConfiguration : DbMigrationsConfiguration<DomainContext>
    //{
    //    public DomainContextMigrationsConfiguration()
    //    {
    //        this.AutomaticMigrationsEnabled = true;
    //    }
    //}

    //public interface IDomainContextFactory<T>
    //{
    //    T Create();
    //}

    //public class DomainContextFactory<T> : IDomainContextFactory<T> where T : new()
    //{
    //    private readonly IConfigReader _configReader;

    //    public DomainContextFactory(IConfigReader configReader)
    //    {
    //        _configReader = configReader;
    //    }


    //    public T Create()
    //    {
    //        return new T(_configReader.ConnectionString);
    //    }
    //}
}
