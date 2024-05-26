using EntityLayer.ErrorModel;
using EntityLayer.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using ServicesLayer.Contracts;
using System.Net;

namespace BookAPI.Extensions
{
    public static class ExceptionMiddlewareEctensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature is not null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch 
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            _ => StatusCodes.Status500InternalServerError
                        };
                        string errorMessage = $"An Erro Occurded: {contextFeature.Error}";
                        logger.LogError(errorMessage);
                        await context.Response.WriteAsync(
                            new ErrorDetails() 
                            { StatusCode = context.Response.StatusCode,
                                Message = contextFeature.Error.Message
                            }.ToString()
                        );

                    }
                });
            });
        }
    }
}
