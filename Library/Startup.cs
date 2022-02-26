using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models.Options;
using Library.Customizations.ModelBinders;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Library.Models.Services.Database;
using Library.Models.Services.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Library.Models.Entities;
using Library.Models.Authorizations;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Library
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
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
            });
            services.AddRazorPages();
            services.AddTransient<IBookService, EfCoreBookService>();
            services.AddTransient<IChapterService, EfCoreChapterService>();
            services.AddTransient<IPaymentGateway, PayPalPaymentGateway>();
            services.AddSingleton<IBookCoverPersister, MagickNetImagePersister>();
            services.AddScoped<IAuthorizationHandler, BookBuyerRequirementHandler>();
            services.AddDbContext<LibraryDbContext>(optionsBuilder =>
            {
                string connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default");
                optionsBuilder.ConfigureWarnings(builder => builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning));
                optionsBuilder.UseSqlServer(connectionString, options =>
                {
                    options.EnableRetryOnFailure(5);
                });
            });
            services.Configure<ConnectionStringsOptions>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<BooksOptions>(Configuration.GetSection("Books"));
            services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));
            services.Configure<UsersOptions>(Configuration.GetSection("Users"));
            services.Configure<PayPalOptions>(Configuration.GetSection("PayPal"));
            services.AddDefaultIdentity<ApplicationUser>()
            .AddEntityFrameworkStores<LibraryDbContext>();
            services.AddAuthorization(option =>
                option.AddPolicy("BookBuyer", builder => builder.Requirements.Add(new BookBuyerRequirement()))
                );
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(routeBuilder =>
            {
                routeBuilder.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();
                routeBuilder.MapRazorPages().RequireAuthorization();
            });
        }
    }
}
