using AutoMapper;
using Core.Query;
using Core.Services.Abstraction;
using Dtos.Dtos;
using IG.Core.Data.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using service.server.HelperClasses;
using service.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers
{
    public class SearchPersonsQueryHandler : IRequestHandler<SearchPersonsQuery, IEnumerable<ReadPersonData>>
    {
        private readonly IPersonsRepo _personsRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchPersonsQueryHandler> _logger;
        public SearchPersonsQueryHandler(IPersonsRepo personsRepo,IMapper mapper, ILogger<SearchPersonsQueryHandler> logger)
        {
            _logger = logger;
            _personsRepo = personsRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ReadPersonData>> Handle(SearchPersonsQuery request, CancellationToken cancellationToken)
        {
        var orderByFunctions = new Dictionary<person, Func<IQueryable<Person>, IOrderedQueryable<Person>>>
        {
            { person.FirstNameENG, q => q.OrderBy(p => p.FirstNameENG) },
            { person.LastNameENG, q => q.OrderBy(p => p.LastNameENG) },
            { person.FirstNameGEO, q => q.OrderBy(p => p.FirstNameGEO) },
            { person.LastNameGEO, q => q.OrderBy(p => p.LastNameGEO) },
            { person.PhoneNumber, q => q.OrderBy(p => p.PhoneNumber) },
            { person.PersonalNumber, q => q.OrderBy(p => p.PersonalNumber) },
        };

                // Determine the orderByFunc using the dictionary or fallback to a default order
                var orderByFunc = orderByFunctions.GetValueOrDefault(request.OrderPersonsBy, q => q.OrderBy(p => p.FirstNameENG));

                var predicate = GenericExpressionTree.CreateWhereClause<Person>(request.SearchPersonsBy.ToString(), request.SearchValue);

                var persons = await _personsRepo.GetByQueryAsync(predicate, orderByFunc);

                var thePersons = persons.Select(p => _mapper.Map<ReadPersonData>(p));

                var result = PagedList<ReadPersonData>
                    .ToPagedList(thePersons.AsQueryable(), request.pagingParameters.PageNumber, request.pagingParameters.PageSize);
                
            return result;
        }
    }
}
