using CalcWebApi.V1.Requests;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CalcWebApi.Validation
{
    public class IncomingValuesValidator : AbstractValidator<IncomingValues>
    {
        public IncomingValuesValidator()
        {
            RuleFor(iv => iv.firstValue).NotNull().WithMessage("First value is required.");
            RuleFor(iv => iv.secondValue).NotNull().WithMessage("Second value is required11.");
            RuleFor(iv => iv.firstValue).Must(BeValidDouble).WithMessage("First value must be a valid number.");
            RuleFor(iv => iv.secondValue).Must(BeValidDouble).WithMessage("Second value must be a valid number.");
        }

        private bool BeValidDouble(double value)
        {
            string stringValue = value.ToString();
            if (!double.TryParse(stringValue, out _))
                return false;
            return !Regex.IsMatch(stringValue, @"[a-zA-Z]");
        }
    }
}
