using Fridge.Data.Models;
using System.Net;

namespace Fridge.Utils.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                int statusCode = 505;
                string exceptionMessage = "Internal Server Error";
                
                if (ex is ArgumentException)
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                    exceptionMessage = ex.Message;
                }

                var errorDetails = new ErrorDetails
                {
                    StatusCode = statusCode,
                    Message = exceptionMessage,
                };

                await HandleExceptionAsync(context, errorDetails);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, ErrorDetails errorDetails)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorDetails.StatusCode;

            return context.Response.WriteAsync(errorDetails.ToString());
        }
    }
}
