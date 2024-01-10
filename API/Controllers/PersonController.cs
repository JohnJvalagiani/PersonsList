using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Core.Services.Abstraction;
using Dtos.Dtos;
using FluentValidation;
using IG.Core.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
        private readonly IPersonManagementsService _personManagementService;
        private readonly IStringLocalizer<PersonController> _localizer;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonManagementsService personManagementService,
            IStringLocalizer<PersonController> localizer,ILogger<PersonController> logger)
        {
            _personManagementService = personManagementService;
            _localizer = localizer;
            _logger = logger;
        }

        [HttpGet("Localization")]
        public IActionResult GetCurrentCultureDate()
        {

            var guid = Guid.NewGuid();

            return Ok(_localizer["RandomGUID", guid.ToString()].Value);

        }

        [HttpPost("Search")]
        public async Task<IActionResult> Search( DetailedSearchParameters detailSearchParameters)
        {
            var result = await _personManagementService.Search(detailSearchParameters);

            if (result.Success != true)
                return NotFound(result);

            return Ok(result);
        }


        [HttpPost("Get All Person")]
        public async Task<IActionResult> GetAllPerson([FromBody]PagingParameters pagingParameters)
        {
                _logger.LogInformation("Getting All Persons");

                var persons = await _personManagementService.GetAllPersons(pagingParameters);

                return Ok(persons);
        }


        [HttpPost("Add person")]
        public async Task<IActionResult> AddPerson([FromBody]CreatePerson person) 
        {

          _logger.LogInformation("Adding Person ");

          var newPerson=await  _personManagementService.AddPerson(person);
              
          return Created(nameof(Person), newPerson);
        }

        [HttpGet("Get Person By Id {id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            _logger.LogInformation( "Get  Person {Id}", id);

            var thePerson = await _personManagementService.GetPersonById(id);
          
            return Ok(thePerson);
        }

        [HttpDelete("Delete Person {id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
         
                _logger.LogInformation( "Delete  Person {Id}", id);
                
               var person = await _personManagementService.DeletePerson(id);

                return NoContent();
        }


        [HttpPut("Update Person")]
        public async Task<IActionResult> EditPerson([FromBody] UpdatePerson person)
        {
            _logger.LogInformation( "Update  Person {Id}", person.Id);

            var thePerson = await _personManagementService.UpdatePerson(person);

             return Ok(thePerson);
        }

        [HttpGet("Get  Persons Report")]
        public async Task<IActionResult> GetPersonsReport( ConnectedPersonType connectedPersonType)
        {
            var result = await _personManagementService.GetConnectedPersonsReport( connectedPersonType);

            return Ok(result);
        }
    }
}
