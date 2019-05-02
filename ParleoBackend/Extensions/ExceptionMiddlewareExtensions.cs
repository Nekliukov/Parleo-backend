using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Parleo.BLL.Exceptions;
using System;
using System.Net;

namespace ParleoBackend.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
 
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        var responseFormat = new ErrorResponseFormat(
                            $"{contextFeature.Error.GetType().Name} - " +
                            $"{contextFeature.Error.Message}");
                        await context.Response.WriteAsync(responseFormat.ToString());
                    }
                });
            });
        }
    }
}
