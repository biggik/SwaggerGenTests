using FluentValidation;
using FluentValidation.Validators;
using SwaggerGenTests.Models;

namespace SwaggerGenTests.Validators
{
    public class CityValidator : IPropertyValidator<WeatherPreferencesSet, string>, ILengthValidator
    {
        public string Name => nameof(CityValidator);

        public int Min => 3;

        public int Max => 50;

        public string GetDefaultMessageTemplate(string errorCode)
        {
            return $"'{{PropertyName}}' must be between {Min} and {Max} characters";
        }

        public bool IsValid(ValidationContext<WeatherPreferencesSet> context, string value)
        {
            if (value is null)
            {
                return false;
            }

            if (value.Length < Min || value.Length > Max)
            {
                return false;
            }

            return true;
        }
    }
}