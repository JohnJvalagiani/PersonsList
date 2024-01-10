using Core.Models;
using Dtos.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commands
{
    public class AddConnectedPersonCommand : IRequest<ServiceResponse<ReadConnectedPerson>>
    {
        public int Id { get; set; }
        public WriteConnectedPerson ConnectedPerson { get; set; }
    }
}
