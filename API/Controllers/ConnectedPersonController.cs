using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Core.Services.Abstraction;
using Dtos.Dtos;
using IG.Core.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using service.server.HelperClasses;

namespace service.server.Controllers
{
    [Route("api/ConnectedPerson")]
    public class ConnectedPersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPersonManagementsService _personManagementsService;
        private readonly ILogger<ConnectedPersonController> _logger;
     
        public ConnectedPersonController(IMediator mediator,ILogger<ConnectedPersonController> logger,IPersonManagementsService personManagementsService)
        {
           _personManagementsService = personManagementsService;
            _mediator = mediator;
            _logger = logger;
        } 

        [HttpPost("Add Connected Person")]
        public async Task<IActionResult> Add(int personId, WriteConnectedPerson connectedPerson)
        {
              _logger.LogInformation("Add Connected Person {Id}", personId);

              var result = await _personManagementsService.AddConnectedPerson(personId,connectedPerson);
           
              return Created(nameof(ConnectedPerson), connectedPerson);
        }

        [HttpDelete("Remove Connected Person")]
        public async Task<IActionResult> Remove(int connectedPersonId)
        {
                _logger.LogInformation(message: "Delete Connected Person {Id}", connectedPersonId);

                var result = await _personManagementsService.RemoveConnectedPerson(connectedPersonId);

                return NoContent();
        }

        }

}
