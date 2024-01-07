using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Core.Services.Abstraction;
using Dtos;
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
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonManagementsService _personManagmentService;
        private readonly IStringLocalizer<PersonController> _localizer;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonManagementsService personManagmentService,
            IStringLocalizer<PersonController> localizer,ILogger<PersonController> logger)
        {
            this._personManagmentService = personManagmentService;

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
        public async Task<IActionResult> Search( DetailedSearchParametrs detailSearchParametrs)
        {


            var result =await _personManagmentService.Search(detailSearchParametrs);


            return Ok(result);

        }


        [HttpPost("Get All Person")]
        public async Task<IActionResult> GetAllPerson([FromBody]PagingParametrs pagingParametrs)
        {

                _logger.LogInformation("Getting All Persons");



                var persons = await _personManagmentService.GetAllPersons(pagingParametrs);

                
             
                return Ok(persons);


        }


        [HttpPost("Add person")]
        public async Task<IActionResult> AddPerson([FromBody]CreatePerson person) {

          _logger.LogInformation("Adding Person ");


          var newPerson=await  _personManagmentService.AddPerson(person);

              
          return Created(nameof(Person), newPerson);
                

        }

        [HttpGet("Get Person By Id {id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            _logger.LogInformation( "Get  Person {Id}", id);


            var theperson = await _personManagmentService.GetPersonById(id);

          
            return Ok(theperson);


        }

        [HttpDelete("Delete Person {id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {

        
         
                _logger.LogInformation( "Delete  Person {Id}", id);
                

               var person = await _personManagmentService.DeletePerson(id);

               

                return NoContent();

           

        }


        [HttpPut("Update Person")]
        public async Task<IActionResult> EditPerson([FromBody] UpdatePerson person)
        {

            _logger.LogInformation( "Update  Person {Id}", person.Id);


            var thePerson = await _personManagmentService.UpdatePerson(person);


             return Ok(thePerson);

           

        }

        [HttpGet("Get  Persons Report")]
        public async Task<IActionResult> GetPersonsReport( ConnectedPersonType conectedPersonType)
        {

            var result = await _personManagmentService.GetConnectedPersonsReport( conectedPersonType);

            return Ok(result);

        }
    }
}
