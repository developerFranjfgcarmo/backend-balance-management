using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BalanceManagement.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BalanceManagement.Api.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorsInModelState = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(s => s.ErrorMessage)).ToArray();

                var errors = new List<ErrorResponse>();

                foreach (var error in errorsInModelState)
                    errors.AddRange(error.Value.Select(subError => new ErrorResponse
                        {FieldName = error.Key, Message = subError}));

                context.Result = new BadRequestObjectResult(errors);
                return;
            }

            await next();

            // after controller
        }
    }
}