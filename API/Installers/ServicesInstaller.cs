using Core.Models;
using Core.Services;
using Core.Services.Abstraction;
using Core.Services.Implementation;
using Domain.Interfaces.Repository;
using Infrastructure.Data.EFCore.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using service.server.Profiles;
using service.server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Service.Server.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallerService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<PersonValidation>();

            services.AddScoped<ConnectedPersonValidation>();

            services.AddScoped(typeof(IPersonsRepo<>), typeof(Repo<>));

            services.AddScoped< IUnitOfWork , UnitOfWork>();

            services.AddScoped<IPersonManagementsService, PersonManagementsService>();

            services.Configure<FilePathConfig>(configuration.GetSection("FilePathConfig"));

            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<FilePathConfig>>().Value);

        }
    }
}
