using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Http;

namespace Crif.Api
{
    public class ValidateRequestFilter : IActionFilter
    {
        private const string RequestInvalidErrorType = "request_invalid";
        private readonly IValidatorFactory _validatorFactory;

        public ValidateRequestFilter(IValidatorFactory validatorFactory)
        {
            if (validatorFactory == null) throw new ArgumentNullException(nameof(validatorFactory));
            _validatorFactory = validatorFactory;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (ControllerParameterDescriptor parameter in context.ActionDescriptor.Parameters)
            {
                var typeInfo = parameter.ParameterType.GetTypeInfo();

                if (typeInfo.IsClass && context.ActionArguments.TryGetValue(parameter.Name, out object parameterValue))
                {
                    // TODO - refactor this
                    if (parameterValue == null && !parameter.ParameterInfo.IsOptional)
                    {
                        context.Result = new ObjectResult(
                            new ErrorResponse(context.HttpContext.TraceIdentifier, RequestInvalidErrorType, new[] { "request_body_required" })
                        ) 
                        {
                            StatusCode = StatusCodes.Status422UnprocessableEntity
                        };

                        return;
                    }

                    if (parameterValue == null) return;

                    var validationResult = Validate(parameter.ParameterType, parameterValue);
                    if (validationResult != null && !validationResult.IsValid)
                    {
                        context.Result = CreateErrorResponse(context.HttpContext.TraceIdentifier, validationResult);
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        private ValidationResult Validate(Type parameterType, object parameterValue)
        {
            var validator = _validatorFactory.GetValidator(parameterType);

            if (validator == null)
                return null;

            return validator.Validate(parameterValue);
        }

        private IActionResult CreateErrorResponse(string requestId, ValidationResult validationResult)
        {
            var errorCodes = validationResult.Errors.Select(e => e.ErrorCode);
            var errorResponse = new ErrorResponse(requestId, RequestInvalidErrorType, errorCodes);
            return new ObjectResult(errorResponse) { StatusCode = StatusCodes.Status422UnprocessableEntity };
        }
    }
}