using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ArrayScorer.Exceptions
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Handle bad requests
                if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
                {
                    await HandleBadRequest(context);
                }
                else
                {
                    await HandleException(context, ex);
                }
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            // Log the exception for troubleshooting
            _logger.LogError(ex, "An unhandled exception occurred.");

            // Customize the response for exceptions
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            // Write a response message
            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
        }

        private async Task HandleBadRequest(HttpContext context)
        {
            // Log or handle bad request scenarios here
            _logger.LogWarning("A bad request occurred.");

            // Customize the response for bad requests
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            // Write a response message for bad requests
            await context.Response.WriteAsync("Bad request. Please check your input.");
        }
    }



}
