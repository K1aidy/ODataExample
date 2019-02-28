using System.Linq;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ODataExample.Context;
using ODataExample.Context.Entity;

namespace ODataExample
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddEntityFrameworkInMemoryDatabase();
			services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("test"));
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddOData();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApiContext context)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			AddTestData(context);

			var builder = new ODataConventionModelBuilder(app.ApplicationServices);

			var users = builder.EntitySet<User>("Users");
			var userinfos = builder.EntitySet<UserInfo>("UserInfos");
				userinfos.EntityType.Property(ui => ui.CityName);
			userinfos.EntityType.Ignore(ui => ui.City);
			//var cities = builder.EntitySet<City>("Cities");

			app.UseMvc(routeBuilder =>
			{
				// Enable full OData queries, you might want to consider which would be actually enabled in production scenaries
				routeBuilder.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

				// Create the default collection of built-in conventions.
				var conventions = ODataRoutingConventions.CreateDefault();

				// Insert the custom convention at the start of the collection.
				//conventions.Insert(0, new NavigationIndexRoutingConvention());

				routeBuilder.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel(), new DefaultODataPathHandler(), conventions);

				// Work-around for #1175
				routeBuilder.EnableDependencyInjection();
			});
		}

		private void AddTestData(ApiContext context)
		{
			context.Cities.AddRange(
				new City { Id = 1, CityName = "Moscow" },
				new City { Id = 2, CityName = "Novosibirsk" });

			context.UserInfos.AddRange(
				new UserInfo { Id = 1, Inn = "1234567890", CityId = 1 },
				new UserInfo { Id = 2, Inn = "0987654321", CityId = 2 });

			context.Users.AddRange(
				new User { Id = 1, Name = "Dmitry", InfoId = 1 },
				new User { Id = 2, Name = "Potap", InfoId = 2 });

			context.SaveChanges();
		}
	}
}
