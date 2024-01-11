using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Core.Commands;
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
        private readonly ILogger<ConnectedPersonController> _logger;
     
        public ConnectedPersonController(IMediator mediator,ILogger<ConnectedPersonController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        } 

        [HttpPost("AddConnectedPerson")]
        public async Task<IActionResult> Add(AddConnectedPersonCommand addConnectedPersonCommand)
        {
            _logger.LogInformation($"Adding connected person for person with ID: {addConnectedPersonCommand.Id}");

            var result = await _mediator.Send(addConnectedPersonCommand);

            _logger.LogInformation($"Added connected person for person with ID {addConnectedPersonCommand.Id}");

            return Created(nameof(ReadConnectedPerson), result);
        }

        [HttpDelete("RemoveConnectedPerson")]
        public async Task<IActionResult> Remove(RemoveConnectedPersonCommand removeConnectedPersonCommand)
        {
            _logger.LogInformation($"Removing connected person with ID: {removeConnectedPersonCommand.ConnectedPersonId} from person with ID: {removeConnectedPersonCommand.PersonId}");

            var result = await _mediator.Send(removeConnectedPersonCommand);

            _logger.LogInformation($"Removed connected person with ID: {removeConnectedPersonCommand.ConnectedPersonId} from person with ID: {removeConnectedPersonCommand.PersonId}");

            return NoContent();
        }

        }

}
