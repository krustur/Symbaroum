using System.Data.Entity.Infrastructure;
using Autofac;
using Symbaroum.Core.Common;
using Symbaroum.Core.Common.Database;

namespace Symbaroum.Core.Infrastructure
{
        public static class Bootstrap
        {
            public static IContainer Container { get; set; }

            public static void Run()
            {
                var builder = new ContainerBuilder();

                builder.RegisterType<ConfigReader>().As<IConfigReader>();
                builder.RegisterType<DomainContextFactory>().As<IDomainContextFactory>();
                builder.RegisterType<DesignTimeDomainContextFactory>().As<IDbContextFactory<DomainContext>>();
                
                Container = builder.Build();
            }
        }
}