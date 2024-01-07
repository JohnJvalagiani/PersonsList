using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Server.Installers
{
    public interface IInstaller
    {

        void InstallerService(IServiceCollection serviceProviderCollection, IConfiguration configuration);
       
    }
}
