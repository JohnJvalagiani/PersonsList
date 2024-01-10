using Core.Models;
using Dtos.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Query
{
    public class GetPersonByIdQuery : IRequest<ReadPersonData>
    {
        public int PersonId { get; set; }
    }
}
