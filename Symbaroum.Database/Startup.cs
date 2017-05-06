using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Symbaroum.Database.Configuration;
using Symbaroum.Infrastructure.Configuration;

namespace Symbaroum.Database
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            var swaggerConfig = services.ConfigureAddScoped<SwaggerConfig>(Configuration.GetSection("Swagger"));            
            var domainContextConfig = services.ConfigureAddScoped<DomainContextConfig>(Configuration.GetSection("DomainContext"));

            // Add framework services.
            services.AddMvc();
            services.AddDbContext<DomainContext>(o =>
            {
                o.UseSqlServer(domainContextConfig.ConnectionString);
            });
            services.AddSwaggerGen(x =>
            {
            });
            services.ConfigureSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Symbaroum.Database API",
                        Version = "v1",
                        Description = "API for accessing the Symbaroum.Database",
                        TermsOfService = "None"
                    }
                );

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, swaggerConfig.XmlCommentsFileName);
                options.IncludeXmlComments(filePath);
                options.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });
        }
    }
}
