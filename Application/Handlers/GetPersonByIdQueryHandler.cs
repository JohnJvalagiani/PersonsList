using AutoMapper;
using Core.Models;
using Core.Query;
using Core.Services.Abstraction;
using Dtos.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, ReadPersonData>
    {
        private readonly IPersonsRepo _personsRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPersonByIdQueryHandler> _logger;


        public GetPersonByIdQueryHandler(
            IPersonsRepo personsRepo,
            IMapper mapper,
            ILogger<GetPersonByIdQueryHandler> logger
          )
        {
            _personsRepo = personsRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ReadPersonData> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _personsRepo.GetByIdAsync(request.PersonId);
            return  _mapper.Map<ReadPersonData>(result);
        }
    }
}
