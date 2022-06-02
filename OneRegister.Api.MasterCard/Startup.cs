using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OneRegister.Api.MasterCard.Filters;
using OneRegister.Api.Service.Abstract.Authorization;
using OneRegister.Api.Service.Abstract.Services;
using OneRegister.Api.Service.Authorization;
using OneRegister.Api.Service.Model;
using OneRegister.Api.Service.Services;
using OneRegister.Data.Context;
using OneRegister.Data.Identication;
using Serilog.Context;
using System;
using System.IO;
using System.Reflection;

namespace OneRegister.Api.MasterCard
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
            services.AddDbContext<OneRegisterContext>(
                o=> o.UseSqlServer(Configuration.GetConnectionString("OneRegisterConnection"))
                );
            services.AddIdentity<OUser, ORole>()
                    .AddEntityFrameworkStores<OneRegisterContext>()
                    .AddDefaultTokenProviders();
            services.AddControllers();
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1",
                        new OpenApiInfo
                        {
                            Title = "OneRegister MasterCard API",
                            Contact = new OpenApiContact
                            {
                                Name = "MobilityOne Development Group",
                                Email = "nader@mobilityonegroup.com"
                            },
                            Description = "OneRegister API for MasterCard project",
                            Version = "1.1"
                        });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                setup.IncludeXmlComments(xmlCommentsFullPath);
                setup.OperationFilter<OpenApiAuthorizationHeaderAttribute>();
            });
            services.Configure<TokenOption>(Configuration.GetSection(TokenOption.Position));
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IInquiryService, InquiryService>();
            
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "OneRegisterMasterCardAPI v1.1");
                c.RoutePrefix = String.Empty;
                c.DocumentTitle = "OneRegister MasterCard API";
            });

            app.Use(async (ctx, next) =>
            {
                using (LogContext.PushProperty("IPAddress", ctx.Connection.RemoteIpAddress))
                {
                    await next();
                }
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
