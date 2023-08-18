using CalcWebApi.V1.Requests;
using FluentValidation;

namespace CalcWebApi.Validation
{
    public class IncomingExpressionValidator : AbstractValidator<IncomingExpression>
    {
        private readonly char[] ValidSymbols = { '+', '-', '*', '/', '^', '(', ')', '.' };
        public IncomingExpressionValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Expression).NotEmpty().WithMessage("Expression is required.");
            RuleFor(x => x.Expression).MinimumLength(3).WithMessage("Expression length must be at least 3 characters.");
            RuleFor(x => x.Expression).Must(c => HaveValidFirstCharacter(c.First())).WithMessage("Expression can only start with a digit ( or -");
            RuleFor(x => x.Expression.Last()).Must(HaveValidLastCharacter).WithMessage("Expression can end only with a number or )");
            RuleFor(x => x.Expression).Custom(HaveValidSyntax);

        }
        private bool HaveValidFirstCharacter(char firstChar) =>
            char.IsDigit(firstChar) || firstChar == '(' || firstChar == '-';
            
        private bool HaveValidLastCharacter(char lastChar) =>
            char.IsDigit(lastChar) || lastChar == ')';
        private bool IsValidSymbol(char operation) => 
            ValidSymbols.Contains(operation);
        private string IncorrectSequenceErrorMessage(params char[] chars) =>
            $"Incorrect Sequence: {string.Concat(chars.Where(ch => ch != '\0'))}";
        private bool IsMathOperator(char currentChar) => 
            (currentChar is '-' || currentChar is '+' || currentChar is '*' || currentChar is '/' || currentChar is '^');
        private void HaveValidSyntax(string expression, ValidationContext<IncomingExpression> context)
        {
            string errorMessage = "";
            bool haveMathOperator = false;
            int parenthesisCount = expression.Last() != ')' ? 0 : 1;
            char previousChar = '\0';
            char currentChar = '\0';
            char nextChar = '\0';
            for (int i = 0; i < expression.Length - 1; i++)
            {
                currentChar = expression[i];
                nextChar = expression[i + 1];
                if (!char.IsDigit(currentChar) && !IsValidSymbol(currentChar))
                {
                    errorMessage = $"Symbol: {currentChar} is not alowed";
                    break;
                }
                if (!haveMathOperator && IsMathOperator(currentChar) && i != 0)
                    haveMathOperator = true;
                if (currentChar is '(')
                {
                    parenthesisCount += 1;
                    if (!(char.IsDigit(nextChar) || nextChar is '-' || nextChar is '('))
                        break;
                    if (char.IsDigit(previousChar))
                        break;
                }
                if (currentChar is '^')
                {
                    if (!(char.IsDigit(nextChar) || nextChar is '-' || nextChar is '(')  || !(char.IsDigit(previousChar) || previousChar == ')'))
                        break;
                }
                if (currentChar is '.' && !(Char.IsDigit(previousChar) && char.IsDigit(nextChar)))
                    break;
                if (currentChar is ')')
                {
                    parenthesisCount += 1;
                    if (char.IsDigit(nextChar) || nextChar is '(' || !(char.IsDigit(previousChar) || previousChar == ')'))
                        break;
                }
                if(IsMathOperator(currentChar)  && IsMathOperator(previousChar) && IsMathOperator(nextChar))
                    break;
                if ((currentChar != '-' && IsMathOperator(currentChar)) && (nextChar != '-' && IsMathOperator(nextChar)))
                    break;
                if (currentChar is '-' && (nextChar != '-' && IsMathOperator(nextChar)))
                    break;
                previousChar = currentChar;
            }
            if (nextChar != expression.Last())
                errorMessage = IncorrectSequenceErrorMessage(previousChar, currentChar, nextChar);
            else if (nextChar is ')' && IsMathOperator(currentChar))
                errorMessage = IncorrectSequenceErrorMessage(currentChar, nextChar);
            else if (!haveMathOperator)
                errorMessage = "Must Have at least one operation";
            else if (parenthesisCount % 2 != 0)
                errorMessage = "Parenthesis count should be even";
            if (errorMessage != "")
                context.AddFailure(errorMessage);
        }
    }
}
