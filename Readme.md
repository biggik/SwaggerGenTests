# Swagger Generation Tests

This repo is to demonstrate behavior using https://github.com/micro-elements/MicroElements.Swashbuckle.FluentValidation, and as a discussion point for either correcting this behavior or using alternate strategies for validation adding rules to swagger

## The problem

This sample is a "standard" Web API project with a <code>WeatherController</code>.

I added a [HttpPost] method to the controller that accepts <code>WeatherPreferencesSet</code> which also has a validator <code>WeatherPreferencesSet</code>

The validator has three rules for each of the three properties on the class:

### [WeatherPreferencesSetValidator](/SwaggerGenTests/Validators/WeatherPreferencesSetValidator.cs)

<code>

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

</code>

### The resulting spec
![Swagger spec](Images\SwaggerSpec.png "Swagger spec")

#### [CityValidator](/SwaggerGenTests/Validators/CityValidator.cs)

The city validator applies min and  max lenghts since it a) is an IPropertyValidator and b) since it implements ILengthValidator

<code>

    public class CityValidator : IPropertyValidator<WeatherPreferencesSet, string>, ILengthValidator
    {
        public int Min => 3;
        public int Max => 50;

</code>

#### [AddressValidator](/SwaggerGenTests/Validators/AddressValidator.cs)

The address validator doesn't apply any restrictions on the spec event though it applies three different property validators on the property

<code>

    public class AddressValidator : AbstractValidator<string>
    {
        public AddressValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .MaximumLength(50)
                .MinimumLength(10);

</code>

### Our actual example

Our custom AbstractValidator is much more complex than the simple <code>AddressValidator</code> above. It is used in a variety of request classes so the ability to specify it once and then use it with <code>.SetValidator(new xxxx())</code> is, well, nice to say the least 

The example below is what we do in this validator (slightly generalized). Note that within the validator we are using dependency injected classes (requestDataManager here, but more in the actual implementation)

<code>

            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .ValidIdentifier() // custom IPropertyValidator to verify the data
                .NoControlCharacters() // Disallow certain characters (newlines, tabs, etc)
                .MaximumLength(MaxLength) 
                .MinimumLength(MinLength)
                .Must(x => !IsFromKnownRange(requestDataManager, configuration.Value, x, Status.Disabled))
                  .WithMessage(x => $"Id {x} is from a disabled range of identifiers.")
                .Must(x => IsFromKnownRange(requestDataManager, configuration.Value, x, Status.Allowed))
                  .WithMessage(x => $"Id {x} is not from a valid range of identifiers.")
                ;

</code>

### Question?

Is this not something that MicroElements.Swashbuckle.FluentValidation should consider? We are willing to help with the implementation but would need some direction.