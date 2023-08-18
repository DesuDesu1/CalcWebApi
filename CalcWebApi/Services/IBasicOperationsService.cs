using CalcWebApi.V1.Requests;

namespace CalcWebApi.Services
{
    public interface IBasicOperationsService
    {
        public double Divide(IncomingValues values);
        public double Multiply(IncomingValues values);
        public double Pow(IncomingValues values);
        public double Root(IncomingValues values);
        public double Subtract(IncomingValues values);
        public double Sum(IncomingValues values);
    }
}