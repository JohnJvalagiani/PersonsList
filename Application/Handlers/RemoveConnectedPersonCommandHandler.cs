using AutoMapper;
using Core.Commands;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Core.Handlers
{
    public class RemoveConnectedPersonCommandHandler : IRequestHandler<RemoveConnectedPersonCommand, bool>
    {
        private readonly IPersonsRepo _personsRepo;
        public RemoveConnectedPersonCommandHandler(
            IPersonsRepo PersonRepo)
        {
            _personsRepo = PersonRepo ?? throw new ArgumentNullException(nameof(PersonRepo));
        }

        public async Task<bool> Handle(RemoveConnectedPersonCommand request, CancellationToken cancellationToken)
        {
                return await _personsRepo.RemoveConnectedPersonAsync(request.PersonId, request.ConnectedPersonId);
        }

    }
}
