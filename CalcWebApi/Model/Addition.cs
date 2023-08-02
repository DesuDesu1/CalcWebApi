namespace CalcWebApi.Model
{
    public class Addition : Operation
    {
        public double Calculate()
        {
            return firstValue + secondValue;
        }
    }
}
