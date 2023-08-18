using CalcWebApi.V1.Requests;
using System.Text;

namespace CalcWebApi.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IBasicOperationsService _basicOperations;

        public EvaluationService(IBasicOperationsService basicOperations)
        {
            _basicOperations = basicOperations;
        }

        public double EvaluateExpression(IncomingExpression value)
        {
            Stack<double> operandStack = new Stack<double>();
            List<string> orderedTokens = OrderTokens(TokenizeExpression(value.Expression));

            foreach (string token in orderedTokens)
            {
                double number;
                if (double.TryParse(token, out number))
                {
                    operandStack.Push(double.Parse(token));
                }
                else if (IsOperator(token))
                {
                    double operand2 = operandStack.Pop();
                    double operand1 = operandStack.Pop();
                    double result = PerformOperation(token, operand1, operand2);
                    operandStack.Push(result);
                }
            }

            double finalResult = operandStack.Pop();
            if (double.IsInfinity(finalResult))
            {
                throw new ArgumentException("Result is infinity");
            }
            if (double.IsNaN(finalResult))
            {
                throw new ArgumentException("Result is NaN");
            }

            return finalResult;
        }

        private string[] TokenizeExpression(string expression)
        {
            List<string> tokens = new List<string>();
            StringBuilder currentToken = new StringBuilder();

            for (int i = 0; i < expression.Length; i++)
            {
                char currentChar = expression[i];

                if (IsOperator(currentChar))
                {
                    if (currentToken.Length > 0)
                    {
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                    }

                    if (currentToken.Length == 0 && currentChar == '-' && (i == 0 || IsOperator(expression[i - 1]) || expression[i - 1] == '('))
                    {
                        currentToken.Append("-1");
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                        tokens.Add("*");
                    }
                    else
                    {
                        tokens.Add(currentChar.ToString());
                    }
                }
                else if (char.IsDigit(currentChar) || currentChar == '.')
                {
                    currentToken.Append(currentChar);
                }
                else if (currentChar == '(' || currentChar == ')')
                {
                    if (currentToken.Length > 0)
                    {
                        tokens.Add(currentToken.ToString());
                        currentToken.Clear();
                    }

                    tokens.Add(currentChar.ToString());
                }
            }

            if (currentToken.Length > 0)
            {
                tokens.Add(currentToken.ToString());
            }

            Console.WriteLine(string.Join(' ', tokens));
            return tokens.ToArray();
        }

        private List<string> OrderTokens(string[] tokens)
        {
            List<string> orderedTokens = new List<string>();
            Stack<string> operatorStack = new Stack<string>();

            foreach (string token in tokens)
            {
                if (IsNumber(token))
                {
                    orderedTokens.Add(token);
                }
                else if (IsOperator(token))
                {
                    while (operatorStack.Count > 0 && IsOperator(operatorStack.Peek()) && GetPrecedence(token) <= GetPrecedence(operatorStack.Peek()))
                    {
                        orderedTokens.Add(operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                }
                else if (token == "(")
                {
                    operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                    {
                        orderedTokens.Add(operatorStack.Pop());
                    }
                    if (operatorStack.Count > 0 && operatorStack.Peek() == "(")
                    {
                        operatorStack.Pop();
                    }
                }
            }

            while (operatorStack.Count > 0)
            {
                orderedTokens.Add(operatorStack.Pop());
            }
            Console.WriteLine("Ordered tokens: " + String.Join(' ', orderedTokens));
            return orderedTokens;
        }
        private bool IsNumber(string token)
        {
            double number;
            return double.TryParse(token, out number);
        }

        private bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '^';
        }
        private bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/" || token == "^";
        }
        private int GetPrecedence(string token)
        {
            switch (token)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "^":
                    return 3;
                default:
                    return 0;
            }
        }
        private double PerformOperation(string operatorToken, double operand1, double operand2)
        {
            var values = new IncomingValues { firstValue = operand1, secondValue = operand2 };
            switch (operatorToken)
            {
                case "+":
                    return _basicOperations.Sum(values);
                case "-":
                    return _basicOperations.Subtract(values);
                case "*":
                    return _basicOperations.Multiply(values);
                case "/":
                    return _basicOperations.Divide(values);
                case "^":
                    return _basicOperations.Pow(values);
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }
    }
}
