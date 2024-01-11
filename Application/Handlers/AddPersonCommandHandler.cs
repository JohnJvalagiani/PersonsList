using AutoMapper;
using Core.Commands;
using Core.Models;
using Core.Services.Abstraction;
using Dtos.Dtos;
using FluentValidation;
using IG.Core.Data.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Handlers
{
    public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, ReadPersonData>
    {
        private readonly IPersonsRepo _personsRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPersonCommandHandler> _logger;
        private readonly IValidator<Person> _validator;

        public AddPersonCommandHandler(
            IPersonsRepo personsRepo,
            IMapper mapper,
            ILogger<AddPersonCommandHandler> logger,
            IValidator<Person> validator)
        {
            _personsRepo = personsRepo;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<ReadPersonData> Handle(AddPersonCommand request, CancellationToken cancellationToken)
        {
                var thePerson = _mapper.Map<Person>(request.CreatePerson);

                var validateResult = await _validator.ValidateAsync(thePerson);

                await _validator.ValidateAndThrowAsync(thePerson, cancellationToken: cancellationToken);

                if (!validateResult.IsValid)
                {
                _logger.LogError(string.Join(", ", validateResult.Errors.Select(error => error.ErrorMessage)));
                }

                var result = await _personsRepo.AddAsync(thePerson);
                await _personsRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                var response = _mapper.Map<ReadPersonData>(result);

                return response;
        }
    }
}
