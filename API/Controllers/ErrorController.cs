using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using service.server.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service.server.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public MyErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; 
            var code = 500; // Internal Server Error by default

            Response.StatusCode = code;

            return new MyErrorResponse(exception); 
        }
    }
}
