using KCSCommunity.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KCSCommunity.Access.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(InvalidOperationException), HandleInvalidOperationException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.TryGetValue(type, out var handler))
        {
            handler.Invoke(context);
            return;
        }

        if (!context.ExceptionHandled)
        {
            HandleUnknownException(context);
        }
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;
        var details = new ValidationProblemDetails(exception.Errors) { Title = "One or more validation errors occurred." };
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var details = new ProblemDetails() { Title = "The specified resource was not found.", Detail = context.Exception.Message };
        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails { Status = StatusCodes.Status401Unauthorized, Title = "Unauthorized" };
        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status401Unauthorized };
        context.ExceptionHandled = true;
    }
    
    private void HandleInvalidOperationException(ExceptionContext context)
    {
        var details = new ProblemDetails { Status = StatusCodes.Status409Conflict, Title = "A conflict occurred.", Detail = context.Exception.Message };
        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status409Conflict };
        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails { Status = StatusCodes.Status500InternalServerError, Title = "An unexpected error occurred on the server." };
        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status500InternalServerError };
        context.ExceptionHandled = true;
    }
}