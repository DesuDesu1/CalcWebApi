using CalcWebApi.Services;
using CalcWebApi.V1;
using CalcWebApi.V1.Requests;
using MathNet.Symbolics;
using Microsoft.AspNetCore.Mvc;

namespace CalcWebApi.Controllers
{
    [ApiController]
    public class calcController : ControllerBase
    {
        private readonly IBasicOperationsService _basicOperations;
        private readonly IEvaluationService _evaluation;

        public calcController(IBasicOperationsService basicOperations, IEvaluationService evaluation)
        {
            _basicOperations = basicOperations;
            _evaluation = evaluation;
        }
        [HttpGet(ApiRoutes.MathOperations.Addition)]
        public IActionResult Addition([FromQuery] IncomingValues values)
        {
            return Ok(_basicOperations.Sum(values));
        }

        [HttpGet(ApiRoutes.MathOperations.Subtraction)]
        public IActionResult Subtraction([FromQuery] IncomingValues values)
        {
            return Ok(_basicOperations.Subtract(values));
        }

        [HttpGet(ApiRoutes.MathOperations.Multiply)]
        public IActionResult Multiplication([FromQuery] IncomingValues values)
        {
            return Ok(_basicOperations.Multiply(values));
        }
        [HttpGet(ApiRoutes.MathOperations.Divide)]
        public IActionResult Division([FromQuery] IncomingValues values)
        {
            return Ok(_basicOperations.Divide(values));
        }
        [HttpGet(ApiRoutes.MathOperations.Pow)]
        public IActionResult Exponentiation([FromQuery] IncomingValues values)
        {
            return Ok(_basicOperations.Pow(values));
        }
        [HttpGet(ApiRoutes.MathOperations.Root)]
        public IActionResult Root([FromQuery] IncomingValues values)
        {
            return Ok(_basicOperations.Root(values));
        }
        [HttpGet(ApiRoutes.MathOperations.ExpressionEvaluation)]
        public IActionResult ExpressionEvaluation([FromQuery] IncomingExpression exp)
        {
            return Ok(_evaluation.EvaluateExpression(exp));
        }
    }
}