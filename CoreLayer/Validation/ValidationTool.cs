using FluentValidation;

namespace CoreLayer.Validation
{
    /// <summary>
    /// Validate objects with Fluent Validation
    /// </summary>
    public class ValidationTool
    {
        /// <summary>
        /// Validate an object using Fluent Validation
        /// </summary>
        /// <param name="validator">Validator object</param>
        /// <param name="entity">object to be validated</param>
        /// <exception cref="ArgumentNullException">if validator is null</exception>
        /// <exception cref="ValidationException">if object is not valid</exception>
        public static void Validate(IValidator validator,object entity)
        {
            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }
            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
