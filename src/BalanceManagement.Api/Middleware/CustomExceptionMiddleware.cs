using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Serilog;

namespace BalanceManagement.Api.Middleware
{
    /// <summary>
    /// Allow catch all errors of the application and return  an InternalServerError.
    /// The error will be shown in the output or in a log(production)
    /// </summary>
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex).ConfigureAwait(false);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = "Internal Server Error"
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            Log.Fatal(exception, "");
            return context.Response.WriteAsync(result);
        }
    }
}
