namespace MyShuttle.Web.AppBuilderExtensions
{
    using Data;
    using DataContextConfigExtensions;
    using Microsoft.Data.Entity;
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;

    public static class DataContextExtensions
    {
        public static IServiceCollection ConfigureDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            var runningOnMono = Type.GetType("Mono.Runtime") != null;
            bool useInMemoryStore = runningOnMono || configuration["Data:UseInMemoryStore"].Equals("true", StringComparison.OrdinalIgnoreCase);

            services.AddEntityFramework()
                    .AddStore(useInMemoryStore)
                    .AddDbContext<MyShuttleContext>(options =>
                    {
                        options.UseInMemoryDatabase();
                    });

            return services;
        }
    }
}