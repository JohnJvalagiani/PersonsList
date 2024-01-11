using Dtos.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commands
{
    public class RemoveConnectedPersonCommand : IRequest<bool>
    {
        public int ConnectedPersonId { get; set; } 
        public int PersonId { get; set; } 
    }
}
