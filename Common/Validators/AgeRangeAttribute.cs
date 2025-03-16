using System.ComponentModel.DataAnnotations;

namespace Common.Validators
{
    public sealed class AgeRangeAttribute(int minAge, int maxAge) : ValidationAttribute
    {

        private readonly int _minAge = minAge;
        private readonly int _maxAge = maxAge;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime birthday)
            {
                DateTime today = DateTime.UtcNow;
                DateTime minDate = today.AddYears(-_maxAge);
                DateTime maxDate = today.AddYears(-_minAge);

                if (birthday < minDate || birthday > maxDate)
                {
                    return new ValidationResult($"Birthday must be between {_minAge} and {_maxAge} years old.");
                }
            }

            return ValidationResult.Success;
        }
    }
}