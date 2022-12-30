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
                string exceptionMessage = ex.Message;
                await HandleExceptionAsync(context, exceptionMessage);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, string exceptionMessage)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = exceptionMessage,
            }.ToString());
        }
    }
}
