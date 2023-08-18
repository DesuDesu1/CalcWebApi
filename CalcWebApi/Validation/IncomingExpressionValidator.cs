using CalcWebApi.V1.Requests;
using FluentValidation;

namespace CalcWebApi.Validation
{
    public class IncomingExpressionValidator : AbstractValidator<IncomingExpression>
    {
        private int parenthesisCount = 0;
        private readonly char[] ValidSymbols = { '+', '-', '*', '/', '^', '(', ')', '.' };
        private string errorMessage { get; set; }
        private bool haveMathOperator = false;
        public IncomingExpressionValidator()
        {
            RuleFor(x => x.Expression).NotEmpty().WithMessage("Expression is required.");
            RuleFor(x => x.Expression).MinimumLength(3).WithMessage("Expression length must be at least 3 characters.");
            RuleFor(x => x.Expression.First()).Must(HaveValidFirstCharacter).WithMessage("Expression can only start with a digit ( or -");
            RuleFor(x => x.Expression.Last()).Must(HaveValidLastCharacter).WithMessage("Expression can end only with character or )");
            RuleFor(x => x.Expression).Must(HaveValidSyntax).WithMessage("Incorrect Syntax");
        }
        private bool HaveValidFirstCharacter(char firstChar)
        {
            return char.IsDigit(firstChar) || firstChar == '(' || firstChar == '-';
        }
            
        private bool HaveValidLastCharacter(char lastChar)
        {
            if (lastChar == ')')
                parenthesisCount += 1;
            return char.IsDigit(lastChar) || lastChar == ')';
        }
        private bool IsValidSymbol(char operation)
        {
            return ValidSymbols.Contains(operation);
        }
        private void IncorrectSequenceErrorMessage(params char[] chars)
        {
             errorMessage = $"Incorrect sequence: {string.Concat(chars.Where(ch => ch != '\0'))}";

        }
        private bool IsMathOperator(char currentChar)
        {
            return (currentChar is '-' || currentChar is '+' || currentChar is '*' || currentChar is '/' || currentChar is '^');
        }
        private bool HaveValidSyntax(string expression)
        {
            char previousChar = '\0';
            haveMathOperator = false;
            for (int i = 0; i < expression.Length - 1; i++)
            {
                char currentChar = expression[i];
                char nextChar = expression[i + 1];
                bool NextCharIsDigit = char.IsDigit(nextChar);
                bool PreviousCharIsDigit = char.IsDigit(previousChar);
                if (!char.IsDigit(currentChar) && !IsValidSymbol(currentChar))
                {
                    errorMessage = $"Symbol: {currentChar} is not alowed";
                    return false;
                }
                if (!haveMathOperator && IsMathOperator(currentChar) && i!=0)
                {
                    haveMathOperator = true;
                }
                if (currentChar is '(')
                {
                    parenthesisCount += 1;
                    if (!(NextCharIsDigit || nextChar is '-'))
                    {
                        IncorrectSequenceErrorMessage(currentChar, nextChar);
                        return false;
                    }
                    if (PreviousCharIsDigit)
                    {
                        IncorrectSequenceErrorMessage(previousChar, currentChar);
                        return false;
                    }
                }
                if (currentChar is '^' 
                    && !(NextCharIsDigit || nextChar is '-' || nextChar is '(') 
                    || !(PreviousCharIsDigit || previousChar == ')'))
                {
                    IncorrectSequenceErrorMessage(currentChar, nextChar);
                    return false;
                }
                if (currentChar is '.' && !(Char.IsDigit(previousChar) && NextCharIsDigit))
                {
                    IncorrectSequenceErrorMessage(previousChar, currentChar, nextChar);
                    return false;
                }
                if (currentChar is ')')
                {
                    parenthesisCount += 1;
                    if (NextCharIsDigit || nextChar is '(' || !PreviousCharIsDigit)
                    {
                        IncorrectSequenceErrorMessage(previousChar, currentChar, nextChar);
                        return false;
                    }
                }
                if(IsMathOperator(currentChar) && IsMathOperator(previousChar) && IsMathOperator(nextChar))
                {
                    IncorrectSequenceErrorMessage(previousChar, currentChar, nextChar);
                    return false;
                }
                if (currentChar != '-' && IsMathOperator(currentChar) && nextChar != '-' && IsMathOperator(nextChar))
                {
                    IncorrectSequenceErrorMessage(currentChar, nextChar);
                    return false;
                }
                previousChar = currentChar;
            }
            if (!haveMathOperator)
            {
                errorMessage = "Must Have at least one - + * / ^";
                return false;
            }
            if(parenthesisCount % 2 != 0)
            {
                return false;
            }
            return true;
        }
    }
}
