using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using MyShuttle.Web.AppBuilderExtensions;
using MyShuttle.Data;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Identity.EntityFramework;
using MyShuttle.Model;

namespace MyShuttle.Web
{
	public class Startup
	{
		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(appEnv.ApplicationBasePath)
				.AddJsonFile("config.json")
				.AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}
		public IConfiguration Configuration { get; private set; }
		public void ConfigureServices(IServiceCollection services)
		{
			services.ConfigureDataContext(Configuration);

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<MyShuttleContext>()
				.AddDefaultTokenProviders();

			services.ConfigureDependencies();
			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseIISPlatformHandler();
			app.UseIdentity();
			app.ConfigureRoutes();
			app.UseStaticFiles();
			MyShuttleDataInitializer.InitializeDatabaseAsync(app.ApplicationServices).Wait();
		}

		public static void Main(string[] args) => WebApplication.Run<Startup>(args);
	}
}
