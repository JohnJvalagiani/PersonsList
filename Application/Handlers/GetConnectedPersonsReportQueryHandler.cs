using AutoMapper;
using Core.Dtos;
using Core.Models;
using Core.Query;
using Core.Services.Abstraction;
using Domain.Models;
using Dtos.Dtos;
using FluentValidation;
using IG.Core.Data.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public class GetConnectedPersonsReportQueryHandler : IRequestHandler<GetConnectedPersonsReportQuery, IEnumerable<ConnectedPersonsReport>>
    {
        private readonly IPersonsRepo _personsRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPersonCommandHandler> _logger;
       

        public GetConnectedPersonsReportQueryHandler(
            IPersonsRepo personsRepo,
            IMapper mapper,
            ILogger<AddPersonCommandHandler> logger
          )
        {
            _personsRepo = personsRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<ConnectedPersonsReport>> Handle(GetConnectedPersonsReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _personsRepo.GetConnectedPersonsReportByType(request.ConnectingType);
            return report;
        }
    }
}
