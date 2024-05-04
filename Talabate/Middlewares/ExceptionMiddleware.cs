using System.Net;
using System.Text.Json;
using Talabate.Errors;

namespace Talabate.Middlewares
{
    public class ExceptionMiddleware :IMiddleware
    {
       // private RequestDelegate _next;
        private ILogger<ExceptionMiddleware> _Logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> Logger, IWebHostEnvironment env)
        {
         //   _next = next;
            _Logger = Logger;
            _env = env;
        }


        public async Task InvokeAsync(HttpContext httptContext, RequestDelegate _next)
        {
            try
            {
                await _next.Invoke(httptContext);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex.Message);
                httptContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httptContext.Response.ContentType = "application/json";
                var response = _env.IsDevelopment() ?
                    new ApiExcecutionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    :
                     new ApiExcecutionResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


                var json = JsonSerializer.Serialize(response, options);
                await httptContext.Response.WriteAsync(json);
            }
        }
    }
}
