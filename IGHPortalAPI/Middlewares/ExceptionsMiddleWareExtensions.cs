using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IGHportalAPI.Middlewares
{
    public static class ExceptionsMiddleWareExtensions
    {
        public static void ConfigureExcetionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IGHportalAPI v1"));


        }



        public static void ConfigureBuiltInExceptionHandler(this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
                    options =>
                    {
                        options.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                var exc = context.Features.Get<IExceptionHandlerFeature>();
                                if (exc != null)
                                {
                                    await context.Response.WriteAsync(exc.Error.Message);
                                }
                            }
                         );
                    });
            }

        }

    }
}
