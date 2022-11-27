using System;
using Account.Business.Services;
using Account.Data.Extension;
using Account.Data.Repositories;
using Account.Web.Validations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Account.Web
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
            services.AddControllers();
            services.AddContextConfiguration();

            AddSwagger(services);
            ConfigureIoC(services);

            services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", opt => {
                opt.Cookie.Name = "MyCookieAuth";
                opt.LoginPath = "/WeatherForecast";
                opt.AccessDeniedPath = "/NO";
            });

            services.AddAuthorization(option => {
                option.AddPolicy("MustBelongToHR",
                    policy => policy.RequireClaim("Department", "HR"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Account Service {groupName}",
                    Version = groupName,
                    Description = "AccountService API",
                    Contact = new OpenApiContact
                    {
                        Name = "Akroma",
                        Email = string.Empty,
                        Url = new Uri("https://akroma.com")
                    }
                });
            });
        }

        private static void ConfigureIoC(IServiceCollection services)
        {
            services.Scan(scan =>
                scan.FromAssemblyOf<AccountValidation>()
                    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Validation")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.Scan(scan =>
                scan.FromAssemblyOf<AccountService>()
                    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.Scan(scan =>
                scan.FromAssemblyOf<AccountRepository>()
                    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
        }
    }
}