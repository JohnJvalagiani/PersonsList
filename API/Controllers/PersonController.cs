using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Core.Commands;
using Core.Handlers;
using Core.Query;
using Core.Services.Abstraction;
using Dtos.Dtos;
using FluentValidation;
using IG.Core.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using service.server.Dtos;
using service.server.Exceptions;
using service.server.HelperClasses;
using service.server.Models;
using service.server.Services;

namespace service.server.Controllers
{
    [Route("api/Person")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<PersonController> _localizer;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IMediator mediator, IStringLocalizer<PersonController> localizer, ILogger<PersonController> logger)
        {
            _logger = logger;
            _mediator = mediator;
            _localizer = localizer;
        }

        [HttpGet("Localization")]
        public IActionResult GetCurrentCultureDate()
        {
            var guid = Guid.NewGuid();

            return Ok(_localizer["RandomGUID", guid.ToString()].Value);
        }

        [HttpGet("SearchPerson")]
        public async Task<ActionResult<IEnumerable<ReadPersonData>>> SearchPerson([FromQuery] SearchPersonsQuery detailSearchParameters)
        {
            _logger.LogInformation($"Searching for persons with parameters: {JsonConvert.SerializeObject(detailSearchParameters)}");

            var result = await _mediator.Send(detailSearchParameters);

            _logger.LogInformation($"Search result: {JsonConvert.SerializeObject(result)}");

            return Ok(result);
        }

        [HttpGet("GetAllPerson")]
        public async Task<ActionResult<List<ReadPersonData>>> GetAllPerson([FromQuery] GetAllPersonsQuery pagingParameters)
        {
            _logger.LogInformation($"Fetching all persons with parameters: {JsonConvert.SerializeObject(pagingParameters)}");

            var persons = await _mediator.Send(pagingParameters);

            _logger.LogInformation($"Fetched {persons.Count} persons successfully.");

            return Ok(persons);
        }


        [HttpPost("AddPerson")]
        public async Task<ActionResult<ReadPersonData>> AddPerson([FromBody] AddPersonCommand person)
        {
            _logger.LogInformation($"Adding Person: {JsonConvert.SerializeObject(person)}");

            var newPerson = await _mediator.Send(person);

            _logger.LogInformation($"Added Person with ID: {newPerson.Id}");

            return Created(nameof(Person), newPerson);
        }

        [HttpGet("GetPersonById")]
        public async Task<ActionResult> GetPersonById([FromBody] GetPersonByIdQuery getPersonByIdQuery )
        {
            _logger.LogInformation($"Fetching person by ID: {getPersonByIdQuery.PersonId}");

            var thePerson = await _mediator.Send(getPersonByIdQuery);

            _logger.LogInformation($"Fetched person with ID {thePerson.Id}: {JsonConvert.SerializeObject(thePerson)}");

            return Ok(thePerson);
        }

        [HttpDelete("DeletePerson")]
        public async Task<IActionResult> DeletePerson(DeletePersonCommand deletePersonCommand)
        {
            _logger.LogInformation($"Deleting person with ID: {deletePersonCommand.PersonId}");

            await _mediator.Send(deletePersonCommand);

            _logger.LogInformation($"Deleted person with ID: {deletePersonCommand.PersonId}");

            return NoContent();
        }


        [HttpPut("UpdatePerson")]
        public async Task<IActionResult> EditPerson([FromBody] UpdatePersonCommand person)
        {
            _logger.LogInformation($"Updating person with ID: {person.UpdatePerson.Id}");

            var thePerson = await _mediator.Send(person);

            _logger.LogInformation($"Updated person with ID {person.UpdatePerson.Id}: {JsonConvert.SerializeObject(person.UpdatePerson)}");

            return Ok(thePerson);
        }

        [HttpGet("GetPersonsReport")]
        public async Task<IActionResult> GetPersonsReport(GetConnectedPersonsReportQuery getConnectedPersonsReportQuery)
        {
            _logger.LogInformation($"Fetching persons report with parameters: {JsonConvert.SerializeObject(getConnectedPersonsReportQuery)}");

            var result = await _mediator.Send(getConnectedPersonsReportQuery);

            _logger.LogInformation($"Fetched persons report successfully.");

            return Ok(result);
        }
    }
}
