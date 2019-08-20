using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BBLibraryApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BBLibraryApp.Services;
using BBLibraryApp.Entities;
using System.Security.Claims;
using BBLibraryApp.Models;
using BBLibraryApp.Infrastructure;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace BBLibraryApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Unique string for administrator claim type
        const string AdministratorClaimType = "http://BBLibraryApp.com/claims/administrator";

        const string AdministratorOnlyPolicy = "AdministratorOnly";
        const string MembersOnlyPolicy = "MembersOnly";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // Association of the BBLibraryContext with the Connection String 
            services.AddDbContext<BBLibraryContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("BBLibraryApp")));

            // Association of the ApplicationDbContext with the Connection String
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("BBLibraryAppIdentity")));

            // services.AddDefaultIdentity<ApplicationUser>()
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.User.RequireUniqueEmail = true;
                // Password requirements
                config.Password.RequireDigit = true;
                config.Password.RequiredLength = 8;
                config.Password.RequiredUniqueChars = 1;
                config.Password.RequireLowercase = true;
                config.Password.RequireNonAlphanumeric = true;
                config.Password.RequireUppercase = true;
            })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultTokenProviders();

            services.AddHttpContextAccessor();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(options =>
                {
                    options.AllowAreas = true;
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AdministratorOnlyPolicy, policy => policy.RequireClaim(AdministratorClaimType));
                options.AddPolicy(MembersOnlyPolicy, policy =>
                 policy.RequireRole("Member"));
            });

            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, IEmailSender emailSender)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            RotativaConfiguration.Setup(env);

            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            await CreateApplicationUsersAsync(scopeFactory);
        }

        private async Task CreateApplicationUsersAsync(IServiceScopeFactory scopeFactory)
        {
            var scope = scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<ApplicationUserManager>();
            var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            //Create admin user if not existing
            var adminUserName = "AppAdmin";
            ApplicationUser adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = "administrator@yourdomain.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "P@ssw0rd");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error occurred creating" +
                        $" new admin user in Startup.cs.");
                }
            }

            //Add Administrator claim type to the admin user if not has claim
            var admincp = await signInManager.ClaimsFactory.CreateAsync(adminUser);
            if (!admincp.HasClaim(c => c.Type == AdministratorClaimType))
            {
                var result = await userManager.AddClaimAsync(adminUser, new Claim(AdministratorClaimType, string.Empty));
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error occurred adding admin claim " +
                        $"to user in Startup.cs.");
                }
            }
        }
    }
}
