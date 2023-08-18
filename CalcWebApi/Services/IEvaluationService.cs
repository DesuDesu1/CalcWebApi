using CalcWebApi.V1.Requests;

namespace CalcWebApi.Services
{
    public interface IEvaluationService
    {
        double EvaluateExpression(IncomingExpression value);
    }
}