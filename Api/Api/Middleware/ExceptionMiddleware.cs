using Api.Errors;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;

namespace Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger
                                  , IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _next(context);  // if there is no exception , then the request moves to next stage


            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                // this means we are gonna set the status code to 500  internal server error

                // we gonna write our own response
                var respone = _env.IsDevelopment() ?
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                  : new ApiException((int)HttpStatusCode.InternalServerError);
                



                var options = new JsonSerializerOptions { PropertyNamingPolicy=JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(respone, options);

                await context.Response.WriteAsync(json);


            }

        }
    }
}
