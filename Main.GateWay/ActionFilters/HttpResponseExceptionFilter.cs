using Grpc.Core;
using Main.GateWay.ValidationExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Main.GateWay.ActionFilters;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is RpcException ex)
        {
            var fromRpcException = GetFromRpcException(ex);

            var validationProblemDetails = GetValidationProblemDetails(fromRpcException!,
                ex.StatusCode);

            context.Result = new ObjectResult(validationProblemDetails);

            context.ExceptionHandled = true;
        }
    }

    private ValidationProblemDetails GetValidationProblemDetails(
    ModelStateDictionary modelState, StatusCode code)
    {
        var firstError = modelState.Values.SelectMany(v => v.Errors).FirstOrDefault();

        var validationProblemDetails = new ValidationProblemDetails(modelState)
        {
            Status = GetStatus(code),
            Title = firstError?.ErrorMessage,
            Detail = firstError?.ErrorMessage,
            Type = HttpStatusCode.BadRequest.ToString()
        };
        return validationProblemDetails;
    }

    private ModelStateDictionary? GetFromRpcException(RpcException ex)
    {
        var validationErros = ex.GetValidationTrailers();

        if (validationErros.Count == 0) return null;

        var modelState = new ModelStateDictionary();

        validationErros.ForEach(error =>
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        });

        return modelState;
    }
    private int GetStatus(StatusCode code)
    => code switch
    {
        StatusCode.InvalidArgument => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };


    public void OnActionExecuting(ActionExecutingContext context)
    {

    }
}