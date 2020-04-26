using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TribiticaMVP.Models;
using TribiticaMVP.Models.Abstractions;
using TribiticaMVP.Services;

namespace TribiticaMVP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            PrepareDb();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddSessionStateTempDataProvider();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Tribitica", Version = "v1" });
            });

            services.AddDbContext<TribiticaDbContext>(options =>
            {
                options.UseSqlite($"Filename={GetDbFileNameFromSettings()}");
            });
            ////services.AddDefaultIdentity<TribiticaAccount>()
            ////    .AddEntityFrameworkStores<TribiticaDbContext>();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddTransient(typeof(IGoalService<>), typeof(GoalService<>));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TribiticaDbContext>();
                context.Database.EnsureCreated();
            }

            var swaggerOptions = new Options.SwaggerOptions();
            Configuration.GetSection(nameof(Options.SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerOptions.JsonRoute;
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.UIEndpoint);
            });

            app.UseDefaultFiles();
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void PrepareDb()
        {
            TribiticaDbContext.DbFileName = GetDbFileNameFromSettings();

            using (var dbContext = new TribiticaDbContext(new DbContextOptionsBuilder<TribiticaDbContext>().UseSqlite("Filename=" + GetDbFileNameFromSettings()).Options))
            {
                dbContext.Database.EnsureCreated();
                // CreateDebugAccount(dbContext, "yankee");
                CreateDebugAccount(dbContext, "cypress");
            }
        }

        private static void CreateDebugAccount(TribiticaDbContext dbContext, string name)
        {
            if (!dbContext.Accounts.Any(x => x.Name == name))
            {
                var tribiticaAccount = new TribiticaAccount
                {
                    ID = Guid.NewGuid(),
                    Name = name,
                    Password = name,
                    Email = $"{name}@a.bc"
                };
                dbContext.Add(tribiticaAccount);
                dbContext.SaveChanges();
            }
        }

        private string GetDbFileNameFromSettings()
        {
            var dbSettings = Configuration.GetSection("Database")?.GetChildren();
            if (dbSettings != null && dbSettings.Any(x => x.Key == "DbName"))
            {
                return dbSettings.First(x => x.Key == "DbName").Value;
            }
            return null;
        }
    }
}
