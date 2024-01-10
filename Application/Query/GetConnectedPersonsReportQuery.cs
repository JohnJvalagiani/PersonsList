using Core.Models;
using Domain.Models;
using Dtos.Dtos;
using IG.Core.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Query
{
    public class GetConnectedPersonsReportQuery : IRequest<IEnumerable<ConnectedPersonsReport>>
    {
        public ConnectedPersonType ConnectingType { get; set; }
    }
}
