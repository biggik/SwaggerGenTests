using FluentValidation;
using SwaggerGenTests.Models;

namespace SwaggerGenTests.Validators
{
    public class AddressValidator : AbstractValidator<string>
    {
        public AddressValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .MaximumLength(50)
                .MinimumLength(10);
        }
    }
}
