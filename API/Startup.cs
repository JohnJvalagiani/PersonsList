using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using service.server.Services;
using Service.Server.CustomExceptionMiddlewear;
using Service.Server.Extensions;
using Service.Server.Installers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace service.server
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
            services.InstallerServicesInAssembly(Configuration);

            services.AddControllers();
         
            services.AddHttpClient();

            services.AddMvc()
                    .AddFluentValidation(fvc =>
                fvc.RegisterValidatorsFromAssemblyContaining<Startup>());

          
            services.AddSwaggerGen();


            services.AddLocalization(options => options.ResourcesPath = "Resources");


            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var cultures = new List<CultureInfo> {
    new CultureInfo("en"),
    new CultureInfo("ka")
               };
            app.UseRequestLocalization(options => {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });


            app.ConfigurationCustomExceptionMiddleware();

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", " API V1");
            });


            app.UseHttpsRedirection();

            app.UseAuthentication();

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
