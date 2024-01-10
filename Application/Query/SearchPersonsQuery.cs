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
    public class SearchPersonsQuery : IRequest<ServiceResponse<IEnumerable<ReadPersonData>>>
    {
        public DetailedSearchParameters DetailedSearchParameters { get; set; }
    }
}
