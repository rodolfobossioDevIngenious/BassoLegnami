using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using In.Core.Models;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using BassoLegnami.Model.Data;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa;
using In.Core.Configuration;
using BassoLegnami.Model.Plugins;
using System.Text.Json;
using Newtonsoft.Json.Serialization;

namespace BassoLegnami
{
	public class Startup
	{
		public Startup(IWebHostEnvironment env)
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddDbContext<SqlServerApplicationDbContext>()
				.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<SqlServerApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.Configure<ApplicationConfigurations>(Configuration.GetSection("ApplicationConfigurations"));
			services.Configure<List<Job>>(Configuration.GetSection("ApplicationConfigurations:Jobs"));

			services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
			  .AddCookie(options =>
			  {
				  options.ExpireTimeSpan = TimeSpan.FromHours(1);
				  options.SlidingExpiration = true;
				  options.ReturnUrlParameter = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.ReturnUrlParameter;
			  });

			services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
			JobStorage.Current = new Hangfire.SqlServer.SqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));

			services.AddDistributedMemoryCache();

			services.AddSession(options =>
			{
				// Set a short timeout for easy testing.
				options.IdleTimeout = TimeSpan.FromMinutes(60);
				options.Cookie.HttpOnly = true;
			});

			// Add application services.
			//services.AddTransient<IEmailSender, EmailSender>();
			services.AddLocalization(options => options.ResourcesPath = "Resources");
			services.Configure<RequestLocalizationOptions>(options =>
			{
				CultureInfo[] supportedCultures = new[]
				{
					new CultureInfo("it-IT"),
				};

                options.DefaultRequestCulture = new RequestCulture(culture: "it-IT", uiCulture: "it-IT");
                options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
            });

			if (!System.IO.Directory.Exists(UnitOfWork.PLUGINS_FOLDER))
			{
				System.IO.Directory.CreateDirectory(UnitOfWork.PLUGINS_FOLDER);
			}
			List<System.Reflection.Assembly> plugins = PluginAssemblyLoader.LoadPlugins(UnitOfWork.PLUGINS_FOLDER);
			IConfigurationSection elsaSection = Configuration.GetSection("Elsa");

			// Elsa services.
			services
				.AddElsa(elsa => elsa
					.UseEntityFrameworkPersistence(ef =>
					{
						System.Reflection.Assembly migrationsAssembly = typeof(Elsa.Persistence.EntityFramework.SqlServer.SqlServerElsaContextFactory).Assembly;

						ef.UseSqlServer(
							Configuration.GetConnectionString("DefaultConnection"),
							opts => opts.MigrationsAssembly(migrationsAssembly.GetName().Name)
						);
					})
					.AddConsoleActivities()
					.AddHttpActivities(elsaSection.GetSection("Server").Bind)
					.AddQuartzTemporalActivities()
					.AddWorkflowsFrom<Startup>()
					//.AddActivity<Model.Activities.ApproveDocument>()
					.AddActivitiesFrom(plugins)
				);

			services.AddMvc(options => 
			{
				options.ModelBinderProviders.Insert(0, new Extensions.ModelBinders.ModelBinderProvider());
                options.Filters.Add(typeof(ActionFilters.LogActionFilter));
            });

			//services.AddJavaScriptTypeDefinitionProvider<Model.Activities.CustomTypeDefinitionProvider>();

			// Elsa API endpoints.
			services.AddElsaApiEndpoints();

			services.AddHangfireServer();

			// For Dashboard.
			services.AddRazorPages()
				.AddRazorRuntimeCompilation()
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				.AddDataAnnotationsLocalization()
				.AddNewtonsoftJson(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ApplicationConfigurations> conf, IOptionsMonitor<List<Job>> jobs)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			CultureInfo[] supportedCultures = new[]
			{
				new CultureInfo("it-IT"),
			};

			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture(supportedCultures.First()),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			});

			app.Use(async (context, next) =>
			{
				context.Response.Headers.Add("X-Frame-Options", "SameOrigin");
				context.Response.Headers.Add("X-Content-Security-Policy", "allow *; options inline-script eval-script; frame-ancestors 'self'");
				context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
				await next().ConfigureAwait(false);
			});

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseHangfireServer();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("Main", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute("Support", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});

			_AddJobs(jobs.CurrentValue);
			jobs.OnChange(_AddJobs);
		}

		private void _AddJobs(List<Job> jobs)
		{
			using (Hangfire.Storage.IStorageConnection connection = JobStorage.Current.GetConnection())
			{
				List<Hangfire.Storage.RecurringJobDto> recurringJobs = Hangfire.Storage.StorageConnectionExtensions.GetRecurringJobs(connection);

                _AddNewJob<Jobs.Tx.EmailTx>(nameof(Jobs.Tx.EmailTx));

                foreach (Hangfire.Storage.RecurringJobDto canceledRecurringJob in recurringJobs.Where(r => !jobs.Select(t => t.Name).Contains(r.Job.Type.Name)))
				{
					RecurringJob.RemoveIfExists(canceledRecurringJob.Id);
				}

				void _AddNewJob<T>(string name) where T : IJob
				{
					Job job = jobs.FirstOrDefault(j => j.Name == name);
					Hangfire.Storage.RecurringJobDto recurringJob = recurringJobs.FirstOrDefault(j => j.Job.Type.Name == name);

					if (recurringJob != null)
					{
						RecurringJob.RemoveIfExists(recurringJob.Id);
					}

					if (job?.Enabled == true)
					{
						RecurringJob.AddOrUpdate<T>(name, r => r.Execute(job.Data), job.CRON, new RecurringJobOptions() { TimeZone = TimeZoneInfo.Local });
					}
				}
			}
		}
	}
}
