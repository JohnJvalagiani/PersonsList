using AutoMapper;
using Core.Commands;
using Core.Models;
using Core.Services.Abstraction;
using Dtos.Dtos;
using FluentValidation;
using IG.Core.Data.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using service.server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, ReadPersonData>
    {
        private readonly PersonValidation _validator;
        private readonly IPersonsRepo _personsRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePersonCommandHandler> _logger;


        public UpdatePersonCommandHandler(
            IPersonsRepo personsRepo,
            IMapper mapper,
            ILogger<UpdatePersonCommandHandler> logger,
            PersonValidation validator
          )
        {
            _validator = validator;
            _personsRepo = personsRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ReadPersonData> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var thePerson = _mapper.Map<Person>(request.UpdatePerson);

            var validateResult = await _validator.ValidateAsync(thePerson);

            await _validator.ValidateAndThrowAsync(thePerson);

            if (!validateResult.IsValid)
            {

                _logger.LogError(string.Join(", ", validateResult.Errors.Select(error => error.ErrorMessage)));
            }

            thePerson = _personsRepo.Update(thePerson);
            await _personsRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return _mapper.Map<ReadPersonData>(thePerson);
        }
    }
}
