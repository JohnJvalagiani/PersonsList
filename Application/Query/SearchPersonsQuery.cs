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
    public class SearchPersonsQuery : IRequest<IEnumerable<ReadPersonData>>
    {
        public PagingParameters pagingParameters { get; set; }
        public person SearchPersonsBy { get; set; }
        public person OrderPersonsBy { get; set; }
        public string SearchValue { get; set; }
        //public DetailedSearchParameters DetailedSearchParameters { get; set; }
    }
}
