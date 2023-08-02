using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CalcWebApi.ValidationAttributes
{
    public class NonZeroAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((double)value == 0)
                return new ValidationResult("We're not doing this today, Sir.");
            return ValidationResult.Success;
        }
    }
}
