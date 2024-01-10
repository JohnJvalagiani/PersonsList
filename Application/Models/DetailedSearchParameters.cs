using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service.server.Models
{
    public class DetailedSearchParameters
    {
        public PagingParameters pagingParameters { get; set; }
        public person SearchPersonsBy { get; set; }
        public person OrderPersonsBy { get; set; }
        public string SearchValue { get; set; }

    }
}
