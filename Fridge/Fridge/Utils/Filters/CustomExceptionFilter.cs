using Fridge.Data.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Fridge.Utils.Filters
{
    public class CustomExceptionFilter : Attribute, IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            ErrorDetails errorDetails = new ();

            if (context.Exception is ArgumentNullException)
            {
                errorDetails = new ErrorDetails
                {
                    Message = context.Exception.Message,
                    StatusCode = (int)HttpStatusCode.NotFound,
                };
            }

            if (context.Exception is ArgumentException)
            {
                errorDetails = new ErrorDetails
                {
                    Message = context.Exception.Message,
                    StatusCode= (int)HttpStatusCode.BadRequest,
                };
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = errorDetails.StatusCode;

            return context.HttpContext.Response.WriteAsync(errorDetails.ToString());
        }
    }
}
