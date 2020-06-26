using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Simulation.Data.Entities.System;
using Simulation.Data.Repository;
using Simulation.Data.Repository.Abstract;
using Simulation.Data.Repository.EntityFramework;
using Simulation.Data.System;
using System.Security.Claims;

namespace Simulation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup( IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.Bind("RBoard", new SiteConfiguration());

            services.AddDbContext<SiteDbContext>(
                options => options.UseSqlServer(SiteConfiguration.ConnectionString));

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<ILocalizedMessageRepository, EFLocalizedMessageRepository>();
            services.AddTransient<DataManager>();

            services.AddIdentity<SiteUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = SiteConfiguration.PasswordMinLength;
            })
                .AddEntityFrameworkStores<SiteDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "RBoard.Cookie";
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
            });

            services.AddAuthorization(config =>
            {
                //config.AddPolicy("Locale", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Locality));
                config.AddPolicy("Registered", policyBuilder => policyBuilder.RequireRole("Registered"));
                config.AddPolicy("Confirmed", policyBuilder => policyBuilder.RequireRole("Confirmed"));
                config.AddPolicy("Administrator", policyBuilder => policyBuilder.RequireRole("Administrator"));
            });

            services.AddMailKit(configuration => configuration.UseMailKit(Configuration.GetSection("EMail").Get<MailKitOptions>()));

            services.AddControllersWithViews()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider();

            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}
