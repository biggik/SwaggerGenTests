using FluentValidation;
using SwaggerGenTests.Models;

namespace SwaggerGenTests.Validators
{
    public class WeatherPreferencesSetValidator : AbstractValidator<WeatherPreferencesSet>
    {
        public WeatherPreferencesSetValidator()
        {
            // All rules added inline
            RuleFor(x => x.State)
                .NotEmpty()
                .MaximumLength(30)
                .MinimumLength(2);

            // A new IPropertyValidator is used for validation
            RuleFor(x => x.City)
                .SetValidator(new CityValidator());

            // A new AbstractValidator is used for validation
            RuleFor(x => x.Address)
                .SetValidator(new AddressValidator());
        }
    }
}
