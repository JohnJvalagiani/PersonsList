using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Core.Services.Abstraction;
using IG.Core.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using service.server.Dtos;
using service.server.HelperClasses;

namespace service.server.Controllers
{
    [Route("api/ConnectedPerson")]
    [ApiController]
    public class ConnectedPersonController : ControllerBase
    {

        private readonly IPersonManagmentService _personManagmentService;
        private readonly ILogger<ConnectedPersonController> _logger;
     

        public ConnectedPersonController(ILogger<ConnectedPersonController> logger,IPersonManagmentService personManagmentService)
        {
            this._personManagmentService = personManagmentService;

            _logger = logger;
        } 

        [HttpPost("Add Connected Person")]
        public async Task<IActionResult> Add(int personId, WriteConnectedPerson connectedPerson)
        {
              _logger.LogInformation("Add Connected Person {Id}", personId);

              var result = await _personManagmentService.AddConnectinedPerson(personId,connectedPerson);
           
              return Created(nameof(ConnectedPerson), connectedPerson);


         
        }

        [HttpDelete("Remove Connected Person")]
        public async Task<IActionResult> Remove(int connectedPersonId)
        {
           
           
                _logger.LogInformation("Delete Connectined Person {Id}", connectedPersonId);


                var result = await _personManagmentService.RemoveConnectinedPerson(connectedPersonId);


                return NoContent();

          
        }


      

      



        }


    
}
