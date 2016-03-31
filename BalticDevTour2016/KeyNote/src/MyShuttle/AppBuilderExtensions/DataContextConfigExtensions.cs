
namespace MyShuttle.Web.DataContextConfigExtensions
{
    using Microsoft.Data.Entity.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    public static class DataContextConfigExtensions
    {
        
        public static EntityFrameworkServicesBuilder AddStore(this EntityFrameworkServicesBuilder services, bool useInMemoryStore)
        {
            // Add EF services to the services container 
            services.AddInMemoryDatabase();
           

            return services;
        }
    }
}