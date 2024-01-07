using FluentValidation;
using IG.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
   public  class ConnectedPersonValidation: AbstractValidator<ConnectedPerson>
    {

        public ConnectedPersonValidation()
        {
            RuleFor(x => x.PersonType).IsInEnum().NotEmpty();
        }

    }
}
