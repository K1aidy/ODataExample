using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ODataExample.Context;
using ODataExample.Context.Entity;
using Swashbuckle.AspNetCore.Swagger;
using ODataExample.Filters;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNet.OData.Routing;

namespace ODataExample
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddEntityFrameworkInMemoryDatabase();
			services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("test"));
			services.AddOData();
			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Version = "v1.0",
					Title = "ODataExample",
				});
				c.DocumentFilter<CustomSwaggerDocumentFilter>();
			});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApiContext context)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			AddTestData(context);

			var builder = new ODataConventionModelBuilder(app.ApplicationServices);

			builder.EntitySet<User>("Users");
			builder.EntitySet<UserInfo>("UserInfos");
			builder.EntitySet<City>("Cities");

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint(
					"v1/swagger.json",
					"v1.0");
			});

			app.UseMvc(routeBuilder =>
			{
				routeBuilder.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

				var conventions = ODataRoutingConventions.CreateDefault();

				routeBuilder.MapODataServiceRoute(
					"ODataRoute", 
					"odata",
					builder.GetEdmModel(),
					new DefaultODataPathHandler(),
					conventions);

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
