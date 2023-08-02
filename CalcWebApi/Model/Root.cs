using CalcWebApi.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CalcWebApi.Model
{
    public class Root : Operation
    {
        [NegativeIfOdd(nameof(secondValue),
            ErrorMessage = "Отрицательное числа допустимы только при нечетной степени")]
        public override double firstValue { get; init; }

        [NonZero(ErrorMessage = "Не может быть нулем")]
        [Range(0, double.MaxValue, ErrorMessage = "Не может быть отрицательным.")]
        public override double secondValue { get; init; }

        public double Calculate()
        {
            if (firstValue is 0)
                return 0;
            if (Double.IsNegative(firstValue))
                return -Math.Pow(Math.Abs(firstValue), 1 / secondValue);
            return Math.Pow(firstValue, 1 / secondValue);
        }
    }
}
