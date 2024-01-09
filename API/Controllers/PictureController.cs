using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Services.Abstraction;
using IG.Core.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using service.server.Exceptions;
using service.server.HelperClasses;

namespace service.server.Controllers
{
    [Route("api/[controller]")]
    public class PictureController : ControllerBase
    {

        private readonly IPersonManagementsService _personManagementService;
        private readonly ILogger<PictureController> _logger;

        public PictureController(ILogger<PictureController> logger, IPersonManagementsService personManagementService)
        {
             _personManagementService= personManagementService;
             _logger = logger;
        }

        [HttpPost("Add  Person Picture")]
        public async Task<IActionResult> AddPicture(int id,IFormFile formFile)
        {
                _logger.LogInformation( "Add Picture  {Id}", id);
                var picturePath = await _personManagementService.UploadPersonPicture(id,  formFile);
                return Ok(picturePath);
        }

      

        [HttpPost("Update  Person Picture")]
        public async Task<IActionResult> Update(int id, IFormFile formFile)
        {
            _logger.LogInformation("Add Picture  {Id}", id);
            var picturePath = await _personManagementService.UpdatePersonPicture(id, formFile);
            return Ok(picturePath);
        }
    }
}
