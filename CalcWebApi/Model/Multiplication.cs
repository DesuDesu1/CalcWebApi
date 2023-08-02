namespace CalcWebApi.Model
{
    public class Multiplication : Operation
    {
        public double Calculate()
        {
            return firstValue * secondValue;
        }
    }
}
