using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FC.Codeflix.Catalog.Api.Filters;

public class ApiGlobalExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _env;

    public ApiGlobalExceptionFilter(IHostEnvironment env) => _env = env;

    public void OnException(ExceptionContext context)
    {
        var detais = new ProblemDetails();
        var exception = context.Exception;

        if (_env.IsDevelopment())
        {
            detais.Extensions.Add("StackTrace", exception.StackTrace);
        }

        if (exception is EntityValidationException)
        {
            var ex = exception as EntityValidationException;
            detais.Title = "One or more validation errors occurred";
            detais.Status = StatusCodes.Status422UnprocessableEntity;
            detais.Detail = ex?.Message;
            detais.Type = "UnprocessableEntity";
        }
        else if (exception is NotFoundException)
        {
            detais.Title = "Not Found";
            detais.Status = StatusCodes.Status404NotFound;
            detais.Detail = exception?.Message;
            detais.Type = "NotFound";
        }
        else
        {
            detais.Title = "An unexpected error occurred";
            detais.Status = StatusCodes.Status500InternalServerError;
            detais.Detail = exception?.Message;
            detais.Type = "InternalServerError";
        }
        
        context.HttpContext.Response.StatusCode = (int) detais.Status;
        context.Result = new ObjectResult(detais);
        context.ExceptionHandled = true;
    }
}