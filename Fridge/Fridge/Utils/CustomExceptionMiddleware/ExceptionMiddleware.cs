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
            catch (ArgumentNullException ex)
            {
                var errorDetails = SetStatusCodeAndMessage((int)HttpStatusCode.NotFound, ex.Message);

                await HandleExceptionAsync(context, errorDetails);
            }
            catch (ArgumentException ex)
            {
                var errorDetails = SetStatusCodeAndMessage((int)HttpStatusCode.BadRequest, ex.Message);

                await HandleExceptionAsync(context, errorDetails);
            }
            catch (Exception ex)
            {
                var errorDetails = SetStatusCodeAndMessage((int)HttpStatusCode.InternalServerError, ex.Message);

                await HandleExceptionAsync(context, errorDetails); 
            }
        }

        private Task HandleExceptionAsync(HttpContext context, ErrorDetails errorDetails)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorDetails.StatusCode;

            return context.Response.WriteAsync(errorDetails.ToString());
        }

        private ErrorDetails SetStatusCodeAndMessage(int statusCode, string message) =>
            new ErrorDetails
            {
                StatusCode = statusCode,
                Message = message,
            };
    }
}
