using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Symbaroum.Database.Configuration
{
    public static class ServiceCollectionExtentions
    {
        public static TOptions ConfigureAddScoped<TOptions>(this IServiceCollection services, IConfigurationSection configurationSection) where TOptions : class
        {
            services.Configure<TOptions>(configurationSection);
            services.AddScoped(cfg =>
            {
                return cfg.GetService<IOptionsSnapshot<TOptions>>().Value;
            });

            return configurationSection.Get<TOptions>();

        }
    }
}