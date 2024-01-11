using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Commands;
using Core.Services.Abstraction;
using IG.Core.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using service.server.Exceptions;
using service.server.HelperClasses;

namespace service.server.Controllers
{
    [Route("api/[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PictureController> _logger;

        public PictureController(ILogger<PictureController> logger, IMediator mediator)
        {
             _mediator = mediator;
             _logger = logger;
        }

        [HttpPost("AddPersonPicture")]
        public async Task<IActionResult> AddPicture(UploadPersonPictureCommand uploadPersonPictureCommand)
        {
            _logger.LogInformation($"Adding picture for person with ID: {uploadPersonPictureCommand.Id}");

            var picturePath = await _mediator.Send(uploadPersonPictureCommand);

            _logger.LogInformation($"Added picture for person with ID {uploadPersonPictureCommand.Id}: {picturePath}");

            return Ok(picturePath);
        }

      

        [HttpPost("UpdatePersonPicture")]
        public async Task<IActionResult> Update(UpdatePersonPictureCommand updatePersonPictureCommand)
        {
            _logger.LogInformation($"Updating picture for person with ID: {updatePersonPictureCommand.Id}");

            var picturePath = await _mediator.Send(updatePersonPictureCommand);

            _logger.LogInformation($"Updated picture for person with ID {updatePersonPictureCommand.Id}: {picturePath}");

            return Ok(picturePath);
        }
    }
}
