using EntityLayer.Models;
using FluentValidation;

namespace ServicesLayer.ValidationRules.FluentValidation
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.Price).GreaterThan(25);
            RuleFor(book => book.Title).MinimumLength(5);
            RuleFor(book => book.Title).MaximumLength(100);
        }
    }
}
