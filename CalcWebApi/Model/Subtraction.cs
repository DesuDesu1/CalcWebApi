namespace CalcWebApi.Model
{
    public class Subtraction : Operation
    {
        public double Calculate()
        {
            return firstValue - secondValue;
        }
    }
}
