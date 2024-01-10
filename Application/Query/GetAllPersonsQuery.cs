using Core.Models;
using Dtos.Dtos;
using MediatR;
using service.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Query
{
    public class GetAllPersonsQuery : IRequest<List<ReadPersonData>>
    {
        public PagingParameters PagingParameters { get; set; }
    }
}
