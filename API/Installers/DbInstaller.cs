using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Service.Server.Installers
{
    public class DbInstaller : IInstaller
    {
      
        public void InstallerService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>
                   (opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
