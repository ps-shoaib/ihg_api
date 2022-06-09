using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IGHportalAPI.Error;

namespace IGHportalAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware
            (
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env
            )
        {
            this.Next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {

                APIResponse response;

                var statusCode = (int)StatusCodes.Status500InternalServerError;

                string message;
                var exceptionType = ex.GetType();
                if (exceptionType == typeof(UnauthorizedAccessException))
                {
                    statusCode = (int)HttpStatusCode.Forbidden;
                    message = "You are not authorized";
                }
                else
                {
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "some unknown error occured";
                }


                if (env.IsDevelopment())
                {
                    response = new APIResponse(statusCode, message , ex.StackTrace);
                }
                else
                {
                    response = new APIResponse(statusCode, message);
                }

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";


                await context.Response.WriteAsync(response.ToString());
                logger.LogError(ex, ex.Message);

            }



        }

    }
}
