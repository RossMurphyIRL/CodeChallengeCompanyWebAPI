﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Responses;

namespace WebAPI.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errorsInModeState = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                                               .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors
                                               .Select(x => x.ErrorMessage)).ToArray();
                var errorResponse = new ErrorResponse();
                foreach(var error in errorsInModeState)
                {
                    foreach(var subError in error.Value)
                    {
                        var errorModel = new ErrorModel
                        {
                            FieldName = error.Key,
                            Message = subError
                        };
                        errorResponse.Errors.Add(errorModel);
                    }
                }
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            } else
            {
                await next();
            }
        }
    }
}
