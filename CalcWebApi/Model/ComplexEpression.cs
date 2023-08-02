using CalcWebApi.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CalcWebApi.Model
{
    public class ComplexEvaluation
    {
        [MathExpression]
        public string expression { get; set; }
        public string[] TokenizeExpression()
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
                        currentToken.Append(currentChar);
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

            return tokens.ToArray();
        }

        private bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '^';
        }
        /// <summary>
        /// Алгоритм сортировочной станции
        /// </summary>
        public List<string> OrderTokens(string[] tokens)
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

            return orderedTokens;
        }

        private bool IsNumber(string token)
        {
            double number;
            return double.TryParse(token, out number);
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
    }

}
