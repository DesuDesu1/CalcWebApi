using CalcWebApi.V1.Requests;

namespace CalcWebApi.Services
{
    public class BasicOperationsService : IBasicOperationsService
    {
        public double Sum(IncomingValues values)
        {
            double result = values.firstValue + values.secondValue;
            ValidateResult(result);
            return result;
        }

        public double Subtract(IncomingValues values)
        {
            double result = values.firstValue - values.secondValue;
            ValidateResult(result);
            return result;
        }

        public double Multiply(IncomingValues values)
        {
            double result = values.firstValue * values.secondValue;
            ValidateResult(result);
            return result;
        }

        public double Divide(IncomingValues values)
        {
            if (values.secondValue == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }
            double result = values.firstValue / values.secondValue;
            ValidateResult(result);
            return result;
        }

        public double Root(IncomingValues values)
        {
            if (values.secondValue == 0)
            {
                throw new ArgumentException("Cannot calculate the root with a zero exponent.");
            }
            double result;
            if (Double.IsNegative(values.firstValue) )
            {
                if (values.secondValue % 2 == 0)
                    throw new ArgumentException("Cannot calculate the root with a negative base and even exponent.");
                result = -Math.Pow(Math.Abs(values.firstValue), 1 / values.secondValue);
            }
            else
            {
                result = Math.Pow(values.firstValue, 1 / values.secondValue);
            }
            ValidateResult(result);
            return result;
        }

        public double Pow(IncomingValues values)
        {
            double result = Math.Pow(values.firstValue, values.secondValue);
            ValidateResult(result);
            return result;
        }
        private void ValidateResult(double result)
        {
            if (Double.IsNaN(result))
            {
                throw new ArgumentException("Invalid result. Result cannot be NaN.");
            }

            if (Double.IsInfinity(result))
            {
                throw new ArgumentException("Invalid result. Result cannot be infinity.");
            }
        }
    }
}
