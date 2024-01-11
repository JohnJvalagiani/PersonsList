using Core.Commands;
using Core.Handlers;
using Core.Models;
using Core.Query;
using Core.Services;
using Core.Services.Abstraction;
using Domain.Interfaces.Repository;
using Domain.Models;
using Dtos.Dtos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using service.server.Profiles;
using service.server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            services.AddScoped<IPersonsRepo, PersonsRepository>();

            services.Configure<FilePathConfig>(configuration.GetSection("FilePathConfig"));

            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<FilePathConfig>>().Value);

            services.AddTransient<IRequestHandler<AddConnectedPersonCommand, ServiceResponse<ReadConnectedPerson>>, AddConnectedPersonCommandHandler>();

            services.AddTransient< IRequestHandler < AddPersonCommand, ReadPersonData >, AddPersonCommandHandler >();

            services.AddTransient<IRequestHandler<DeletePersonCommand, bool>, DeletePersonCommandHandler>();

            services.AddTransient<IRequestHandler<GetAllPersonsQuery, List<ReadPersonData>>, GetAllPersonsQueryHandler>();

            services.AddTransient<IRequestHandler<GetConnectedPersonsReportQuery, IEnumerable<ConnectedPersonsReport>>, GetConnectedPersonsReportQueryHandler>();

            services.AddTransient<IRequestHandler<GetPersonByIdQuery, ReadPersonData>, GetPersonByIdQueryHandler>();

            services.AddTransient<IRequestHandler<RemoveConnectedPersonCommand, bool>, RemoveConnectedPersonCommandHandler>();

            services.AddTransient<IRequestHandler<SearchPersonsQuery, IEnumerable<ReadPersonData>>, SearchPersonsQueryHandler>();

            services.AddTransient<IRequestHandler<UpdatePersonCommand, ReadPersonData>, UpdatePersonCommandHandler>();

            services.AddTransient<IRequestHandler<UpdatePersonPictureCommand, string>, UpdatePersonPictureCommandHandler>();

            services.AddTransient<IRequestHandler<UploadPersonPictureCommand, string>, UploadPersonPictureCommandHandler>();


        }
    }
}
