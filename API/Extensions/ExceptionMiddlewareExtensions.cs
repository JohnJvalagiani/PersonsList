using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Service.Server.CustomExceptionMiddlewear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Service.Server.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {

        public static  void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if(contextFeature !=null)
                    {
                        logger.LogError($"Something went wrong : {contextFeature.Error}");
                    }

                    await context.Response.WriteAsync(
                        new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error"
                        }.ToString()
                        );

                }


                );
           });


        }

        public static void ConfigurationCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

        }
    }
}
