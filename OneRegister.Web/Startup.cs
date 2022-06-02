using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using OneRegister.Data.Context;
using OneRegister.Security.Middlewares;
using OneRegister.Web.Services.Dependency;
using OneRegister.Web.Services.Setup;
using Serilog;
using Serilog.Context;
using System.IO;

namespace OneRegister.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OneRegisterContext oneRegisterContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Billboard/Error");
            }
            oneRegisterContext.Database.Migrate();
            app.Use(async (ctx, next) =>
            {
                using (LogContext.PushProperty("IPAddress", ctx.Connection.RemoteIpAddress))
                {
                    await next();
                }
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "ExportAppFiles")),
                RequestPath = "/ExportApp",
                ServeUnknownFileTypes = true
            });
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.Use(async (ctx, next) =>
            {
                using (LogContext.PushProperty("SessionId", ctx.Session.Id))
                {
                    await next();
                }
            });
            app.Use(async (ctx, next) =>
            {
                using (LogContext.PushProperty("UserName", ctx.User.Identity.Name))
                {
                    await next();
                }
            });
            app.UseSecurityModule("/Alert/Forbidden");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //seeding
            DataSeeding.Seed(app.ApplicationServices);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(Services.Profiles.WebMapperProfile),
                typeof(Domain.MapperProfiles.DomainMapperProfile));

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
            services.AddHttpClient();

            //Session
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.RegisterContexts(Configuration);
            services.RegisterIdentity();
            services.RegisterSecurityModule();
            services.ConfigureCookies(Configuration);
            services.RegisterOptions(Configuration);
            services.RegisterServices();

            services.RegisterNotificationServices();
            services.RegisterMasterCardJobServices();
            services.ConfigureQuartz(Configuration);
        }
    }
}
