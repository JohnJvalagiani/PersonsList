using FluentValidation;
using IG.Core.Data.Entities;
using service.server.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service.server.Services
{
    public class PersonValidation : AbstractValidator<Person>
    {
        public PersonValidation()
        {
            RuleFor(x => x.FirstNameENG).Length(1, 250).NotEmpty().Matches(@"^[a-zA-Z-']*$").WithMessage("Enter only English Letters"); ;
            RuleFor(x => x.LastNameENG).Length(1,250).NotEmpty().Matches(@"^[a-zA-Z-']*$").WithMessage("Enter only English Letters");
            RuleFor(x => x.FirstNameGEO).Length(1,250).NotEmpty().Matches(@"[^\x00-\x7F]+").WithMessage("Enter only Georgian Letters");
            RuleFor(x => x.LastNameGEO).Length(1,250).NotEmpty().Matches(@"[^\x00-\x7F]+").WithMessage("Enter only Georgian Letters");
            RuleFor(x => x.PhoneNumber).Length(4,50).NotEmpty().Matches("^[0-9]*$");
            RuleFor(x => x.BirthDate).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.PersonalNumber).Length(9, 11).NotEmpty();
            RuleFor(x => x.BirthDate).Must(BeAValidAge).WithMessage("Invalid age. Must be at least 18 years old.");
        }

        private bool BeAValidAge(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            return age >= 18;
        }
    }
}