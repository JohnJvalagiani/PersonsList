using Core.Services;
using Core.Services.Abstraction;
using Core.Services.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddScoped(typeof(IRepo<>), typeof(Repo<>));


            services.AddScoped<IPersonManagmentService, PersonManagmentService>();


        }
    }
}
