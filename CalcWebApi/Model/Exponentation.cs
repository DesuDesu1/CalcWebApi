namespace CalcWebApi.Model
{
    public class Exponentiation : Operation
    {
        public override double secondValue { get; init; }
        public double Calculate()
        {
            return Math.Pow(firstValue, secondValue);
        }
    }
}
