using System.ComponentModel.DataAnnotations;

namespace CalcWebApi.ValidationAttributes
{
    /// <summary>
    /// Велосипед.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MathExpressionAttribute : ValidationAttribute
    {
        private int parenthesisCount = 0;
        private readonly char[] ValidSymbols = { '+', '-', '*', '/', '^', '(', ')', '.' };
        private string ErrorResponse = "";
        private bool atleastoneoperator = false;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string expression = value.ToString()!;
            parenthesisCount = 0;
            var first = expression.FirstOrDefault();
            var last = expression.LastOrDefault();
            if (expression.Length < 3)
            {
                return new ValidationResult($"Длинна выражения не может быть меньше 3");
            }
            if (!IsValidFirstCharacter(first))
            {
                return new ValidationResult($"Первое значение не может быть: {first}");
            }
            if (!IsValidLastCharacter(last))
            {
                return new ValidationResult($"Последний символ не может быть: {last}");
            }
            if (!IsValidSyntax(expression))
            {
                return new ValidationResult(ErrorResponse);
            }
            if (parenthesisCount % 2 != 0)
            {
                return new ValidationResult($"Незакрытая скобка");
            }
            if (!atleastoneoperator)
            {
                return new ValidationResult($"Должен быть хотя бы один математический оператор");
            }
            return ValidationResult.Success;
        }

        private bool IsValidFirstCharacter(char firstChar)
        {
            if (firstChar == '(')
            {
                parenthesisCount += 1;
            }
            return char.IsDigit(firstChar) || firstChar is '(' || firstChar is '-';
        }
        private bool IsValidLastCharacter(char lastChar)
        {
            if (lastChar == ')')
            {
                parenthesisCount += 1;
            }
            return char.IsDigit(lastChar) || lastChar == ')';
        }
        private bool IsValidSymbol(char operation) =>
            ValidSymbols.Contains(operation);
        private void IncorrectSequenceErrorMessage(params char[] chars) => 
            ErrorResponse = $"Некорректная последовательность: {string.Concat(chars.Where(ch => ch != '\0'))}";
        private bool AtLeastOneMathOperator(char currentChar) =>
            (currentChar is '-' || currentChar is '+' || currentChar is '*' || currentChar is '/' || currentChar is '^');
        private bool IsValidSyntax(string expression)
        {
            char previousChar = '\0';
            atleastoneoperator = false;
            for (int i = 0; i < expression.Length - 1; i++)
            {
                char currentChar = expression[i];
                char nextChar = expression[i + 1];
                bool CurrentCharIsDigit = char.IsDigit(currentChar);
                bool NextCharIsDigit = char.IsDigit(nextChar);
                bool PreviousCharIsDigit = char.IsDigit(previousChar);
                if (!CurrentCharIsDigit && !IsValidSymbol(currentChar))
                {
                    ErrorResponse = $"Недопустимый символ: {currentChar}";
                    return false;
                }
                if (!atleastoneoperator && AtLeastOneMathOperator(currentChar))
                {
                    atleastoneoperator = true;
                }
                if (currentChar is '-' && !(NextCharIsDigit || nextChar is '-'))
                {
                    IncorrectSequenceErrorMessage(currentChar, nextChar);
                    return false;
                }
                if ((previousChar is '-'
                    || previousChar is '+'
                    || previousChar is '/'
                    || previousChar is '*'
                    || previousChar is '\0')
                    && currentChar is '-'
                    && !NextCharIsDigit
                    )
                {
                    IncorrectSequenceErrorMessage(previousChar, currentChar, nextChar);
                    return false;
                }
                if ((currentChar is '+' || currentChar is '*' || currentChar is '/')
                    && !(NextCharIsDigit || nextChar is '(' || nextChar is '-'))
                {
                    IncorrectSequenceErrorMessage(currentChar, nextChar);
                    return false;
                }
                if (currentChar is '(')
                {
                    parenthesisCount += 1;
                    if (!(NextCharIsDigit
                    || nextChar is '-'))
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
                if (currentChar is '^' && !(NextCharIsDigit || nextChar is '-' || nextChar is '('))
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
                    if ((NextCharIsDigit || nextChar is '('))
                    {
                        IncorrectSequenceErrorMessage(previousChar, currentChar, nextChar);
                        return false;
                    }
                }
                previousChar = currentChar;
            }
            return true;
        }
    }
}
