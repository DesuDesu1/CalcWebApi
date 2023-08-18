using CalcWebApi.V1.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CalcWebApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentException || context.Exception is DivideByZeroException)
            {
                context.Result = new ObjectResult(new ErrorResponse {
                    Errors = new List<ErrorModel>{new ErrorModel{Message = context.Exception.Message}}
                }){ StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }
    }
}
