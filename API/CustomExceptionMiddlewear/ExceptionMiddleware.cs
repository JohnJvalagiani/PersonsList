using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Service.Server.CustomExceptionMiddlewear
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {

            _next = next;
            _logger = logger;

        }


        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong : {ex}");
                await HandleExceptionAsync(context);

            }

        }

        private Task HandleExceptionAsync(HttpContext context)
        {

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(
                new ErrorDetails
                {
                    StatusCode = context.Response.StatusCode,
                     Message = "Internal Server Error ."
                }
                .ToString());


        }

       
    }
}

