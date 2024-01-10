using AutoMapper;
using Core.Commands;
using Core.Models;
using Core.Services.Abstraction;
using Domain.Interfaces.Repository;
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
    public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, ServiceResponse<ReadPersonData>>
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

        public async Task<ServiceResponse<ReadPersonData>> Handle(AddPersonCommand request, CancellationToken cancellationToken)
        {
            var response = new ServiceResponse<ReadPersonData>();

                var thePerson = _mapper.Map<Person>(request.CreatePerson);

                var validateResult = await _validator.ValidateAsync(thePerson);

                await _validator.ValidateAndThrowAsync(thePerson, cancellationToken: cancellationToken);

                if (!validateResult.IsValid)
                {
                    response.Success = false;
                    response.ErrorMessage = string.Join(", ", validateResult.Errors.Select(error => error.ErrorMessage));
                    return response;
                }

                if (!AgeValidation(request.CreatePerson.BirthDate))
                {
                    response.Success = false;
                    response.ErrorMessage = "Invalid Age";
                    return response;
                }

                var result = await _personsRepo.AddAsync(thePerson);
                await _personsRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                response.Success = true;
                response.Data = _mapper.Map<ReadPersonData>(result);

                return response;
        }

        private static bool AgeValidation(DateTime birthDate)
        {
            var dateTimeNow = DateTime.Now;

            var age = dateTimeNow.AddYears(-birthDate.Year);

            return age.Year > 18;
        }
    }

}
