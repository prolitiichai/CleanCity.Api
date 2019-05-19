using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TrashMap.Api.DataBase;
using TrashMap.Api.DataBase.Creation;
using TrashMap.Api.DataBase.FileStorage;
using TrashMap.Api.DataBase.Providers;
using TrashMap.Api.DataBase.SqlWrapping;
using TrashMap.Api.Formatters;
using TrashMap.Api.Settings;

namespace TrashMap.Api
{
	public class Startup
	{
		private readonly ILogger _logger;

		public Startup(IConfiguration configuration, ILogger<Startup> logger)
		{
			Configuration = configuration;
			_logger = logger;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<IdentityOptions>(options =>
			{
				// Password settings.
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;

				// Lockout settings.
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;

				// User settings.
				options.User.AllowedUserNameCharacters =
				"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = false;
			});

			services
				.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
			{
				// Cookie settings
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

				options.LoginPath = "/api/login";
				options.AccessDeniedPath = "/api/register";
				options.SlidingExpiration = true;
			});

			var dbSettings = new DbSettings() { DataBasePath = "db/main.db", Name = "main", Password = "" };
			var creator = new SQLiteDBCreator(dbSettings, _logger);
			if (!creator.DatabaseExists())
			{
				creator.CreateEmptyDB();
			}
			var connectionHolder = new SQLiteConnectionProvider(dbSettings, _logger);
			DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqliteDialect();
			var database = new Database(connectionHolder.CreateConnection(), new SqlGeneratorImpl(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new[] { typeof(SqliteClassMapping).Assembly }, new SqliteDialect())));

			services.AddSingleton<IFileStorage>(new FileStorage("db/images"));
			services.AddSingleton<IDatabase>(database);
			services.AddSingleton<IUserManager, UserManager>();
			services.AddSingleton<IPointManager, PointManager>();
			services.AddSingleton<IPointCommentEntityManager, PointCommentEntityManager>();
			services.AddSingleton<ILikeManager, LikeManager>();

			services.AddLogging(config =>
			{
				config.AddDebug();
				config.AddConsole();
			});
			services.AddMvc(options =>
			{
				options.InputFormatters.Insert(0, new BinaryInputFormatter());
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseAuthentication();
			//app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
