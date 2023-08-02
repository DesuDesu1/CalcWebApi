using System.ComponentModel.DataAnnotations;

namespace CalcWebApi.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NegativeIfOddAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public NegativeIfOddAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(!Double.IsNegative((double)value))
                return ValidationResult.Success;
            var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
                return new ValidationResult($"Unknown property: {_otherPropertyName}");
            var otherValue = (double)otherProperty.GetValue(validationContext.ObjectInstance);
            if (otherValue % 2 == 0)
                return new ValidationResult(ErrorMessage);
            return ValidationResult.Success;
        }
    }
}
