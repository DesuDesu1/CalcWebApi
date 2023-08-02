using CalcWebApi.ValidationAttributes;

namespace CalcWebApi.Model
{
    public class Division : Operation
    {
        [NonZero(ErrorMessage = "We're not doing this today, Sir.")]
        public override double secondValue { get; init; }

        public double Calculate()
        {
            return firstValue / secondValue;
        }
    }
}
