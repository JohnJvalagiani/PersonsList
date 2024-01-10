using Core.Models;
using Dtos.Dtos;
using MediatR;
using service.server.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commands
{
    public class AddPersonCommand : IRequest<ServiceResponse<ReadPersonData>>
    {
        public CreatePerson CreatePerson { get; set; }
    }
}
