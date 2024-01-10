using AutoMapper;
using Core.Commands;
using Core.CustomExceptions;
using Core.Models;
using Core.Services.Abstraction;
using Domain.Interfaces.Repository;
using Dtos.Dtos;
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
    public class AddConnectedPersonCommandHandler : IRequestHandler<AddConnectedPersonCommand, ServiceResponse<ReadConnectedPerson>>
    {
        private readonly IPersonsRepo _personsRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AddConnectedPersonCommandHandler> _logger;

        public AddConnectedPersonCommandHandler(
            IPersonsRepo personsRepo,
            IMapper mapper,
            ILogger<AddConnectedPersonCommandHandler> logger)
        {
            _personsRepo = personsRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<ReadConnectedPerson>> Handle(AddConnectedPersonCommand request, CancellationToken cancellationToken)
        {
               var response = new ServiceResponse<ReadConnectedPerson>();

                var theConnectedPerson = _mapper.Map<ConnectedPerson>(request.ConnectedPerson);

                var person = await _personsRepo.GetByIdAsync(request.Id);

                if (person == null)
                {
                    _logger.LogError($"Person not found with id {request.Id}");

                    throw new PersonNotFoundException($"Person not found with id {request.Id}");
                }

                theConnectedPerson.PersonId = request.Id;

                person.ConnectedPerson.Add(theConnectedPerson);

                _personsRepo.Update(person);

                await _personsRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                response.Success = true;
                response.Data = _mapper.Map<ReadConnectedPerson>(theConnectedPerson);
                return response;
        }
    }
}
