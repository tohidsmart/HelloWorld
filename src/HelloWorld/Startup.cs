using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Hosting;
using HelloWorld.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using HelloWorld.Models;
using HelloWorld.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNet.Authentication.Cookies;
using System.Net;
using System.Threading.Tasks;
using System;

namespace HelloWorld
{
	public class Startup
	{
		public static IConfigurationRoot configuration;
		public Startup(IApplicationEnvironment env)
		{

			var builder = new ConfigurationBuilder()
				.AddJsonFile("config.json").SetBasePath(env.ApplicationBasePath)
				.AddEnvironmentVariables();
			configuration = builder.Build();
		}


		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(config =>
			{
#if !DEBUG
                config.Filters.Add(new RequireHttpsAttribute());
#endif

			})
			 .AddJsonOptions(opt =>
			 {
				 opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			 });

#if DEBUG
			services.AddScoped<IMailService, DebugMailService>();
#endif
			services.AddEntityFramework().AddSqlServer().AddDbContext<WorldContext>();
			services.AddScoped<CoordService>();
			services.AddTransient<WorldContextSeedDate>();
			services.AddScoped<IWorldRepository, WorldRepository>();
			services.AddIdentity<WorldUser, IdentityRole>(config =>
			{
				config.User.RequireUniqueEmail = true;
				config.Password.RequiredLength = 8;
				config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";

				config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
				{
					OnRedirectToLogin = RedirectToLogin
				};

			}).AddEntityFrameworkStores<WorldContext>();
		}

	
		public async void Configure(IApplicationBuilder app, WorldContextSeedDate data,IHostingEnvironment env)
		{
			//Order does matters
			app.UseStaticFiles();

			app.UseIdentity();

			Mapper.Initialize(config =>
			{
				config.CreateMap<Trip, TripAPIModel>().ReverseMap();
				config.CreateMap<Stop, StopAPIModel>().ReverseMap();
			});
			app.UseMvc(config =>
			{
				config.MapRoute(
				name: "Deafualt",
				template: "{controller}/{action}/{id?}",
				defaults: new { controller = "App", action = "Index" }
				);
			});



			await data.EnsureSeedDataAsync();
		}

		public static void Main(string[] args) => WebApplication.Run<Startup>(args);

		private Task RedirectToLogin(CookieRedirectContext ctx)
		{
			if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == (int)HttpStatusCode.OK)
			{
				ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			}
			else
			{
				ctx.Response.Redirect(ctx.RedirectUri);
			}

			return Task.FromResult(0);
		}

	}
}
