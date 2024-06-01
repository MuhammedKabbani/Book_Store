using CoreLayer.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PresentationLayer.ActionFilters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        private Type _validatorType;
        public Type ValidatorType { 
            get => _validatorType;
            set
            {
                if (!typeof(IValidator).IsAssignableFrom(value))
                {
                    throw new ArgumentException("This is not a validation class");
                }
                _validatorType = value;
            }
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entitiesToValidate = context.ActionArguments.Where(p => p.Value.GetType() == entityType);

            foreach (var entity in entitiesToValidate.Select(x=>x.Value))
            {
                try
                {
                    ValidationTool.Validate(validator, entity);
                }
                catch (ValidationException ex)
                {
                    context.Result = new UnprocessableEntityObjectResult(ex.Message);
                    return;
                }
            }
        }
    }
}
