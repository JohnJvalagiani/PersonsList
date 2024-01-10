using Core.Commands;
using Core.CustomExceptions;
using Core.Models;
using Core.Services.Abstraction;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, bool>
    {
        private readonly IPersonsRepo _personsRepo;
        private readonly ILogger<DeletePersonCommandHandler> _logger;

        public DeletePersonCommandHandler(
            IPersonsRepo personsRepo,
            ILogger<DeletePersonCommandHandler> logger)
        {
            _personsRepo = personsRepo;
            _logger = logger;
        }

        public async Task<bool> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
                var person = await _personsRepo.GetByIdAsync(request.PersonId);

                if (person == null)
                {
                    _logger.LogError($"Person not found with id {request.PersonId}");
                    throw new PersonNotFoundException($"Person not found with id {request.PersonId}");
                }

                await _personsRepo.RemoveAsync(request.PersonId);
                await _personsRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                return true;
        }
    }
}
