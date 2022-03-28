using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web
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
            // Toast Notification Extension Configuration
            services.AddNotyf(config => 
            {
                config.DurationInSeconds = 15;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopCenter;
            });

            // Đăng ký TennisContext
            services.AddDbContext<TennisContext>(options => {
                // Read connection string
                string connectstring = Configuration.GetConnectionString("TennisConnection");
                // User MS SQL Server
                options.UseSqlServer(connectstring);
            });

            // Đăng ký Dịch vụ Identity
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<TennisContext>()
                .AddDefaultTokenProviders();
            //services.AddIdentity<DS_VDV, IdentityRole>()
            //    .AddEntityFrameworkStores<TennisContext>()
            //    .AddDefaultTokenProviders();

            // Truy cập IdentityOptions
            services.Configure<IdentityOptions>(options => {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 0; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = false;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

                // Config Cookie
                services.ConfigureApplicationCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.LoginPath = $"/Account/Login";                                 // Url login page
                    options.LogoutPath = $"/Account/Logout";
                    options.AccessDeniedPath = $"/Account/AccessDenied";
                });
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseNotyf();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Default}/{id?}");
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{action=Index}/{id?}",
                //    defaults: new { area = "NoRole", controller = "NoRole", action = "Index" });

                //pattern: "{area:exists}/{controller=NoRole}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute(
                //  name: "admin",
                //  pattern: "{controller=Account}/{action=Login}/{id?}");
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=NoRole}/{action=Index}/{id?}");
                //endpoints.MapControllers();
            });

        }
    }
}
