using AutoMapper;
using Core.Models;
using Core.Query;
using Core.Services.Abstraction;
using Dtos.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using service.server.HelperClasses;
using service.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public class GetAllPersonsQueryHandler : IRequestHandler<GetAllPersonsQuery, List<ReadPersonData>>
    {
        private readonly IPersonsRepo _personsRepo;
        private readonly ILogger<GetAllPersonsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllPersonsQueryHandler(
            IMapper mapper,
            IPersonsRepo personsRepo,
            ILogger<GetAllPersonsQueryHandler> logger)
        {
            _mapper = mapper;
            _personsRepo = personsRepo;
            _logger = logger;
        }
        public async Task<List<ReadPersonData>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
        {
            var persons = await _personsRepo.GetAllAsync();

            var thePersons = persons.Select(p => _mapper.Map<ReadPersonData>(p));
            var result = PagedList<ReadPersonData>
            .ToPagedList(thePersons.AsQueryable(), request.PageNumber, request.PageSize);

            return result;
        }
    }
}
