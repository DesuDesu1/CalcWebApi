using CalcWebApi.Model;
using MathNet.Symbolics;
using Microsoft.AspNetCore.Mvc;

namespace CalcWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class calcController : ControllerBase
    {
        [HttpGet]
        [Route("addition")]
        public IActionResult Addition([FromQuery] Addition operation)
        {
            if (!TryValidateModel(operation))
            {
                return BadRequest(ModelState);
            }

            var result = operation.Calculate();
            if (double.IsInfinity(result))
                return Ok("inf");
            if (double.IsNaN(result))
                return BadRequest("Nan");
            return Ok(result);
        }

        [HttpGet]
        [Route("subtraction")]
        public IActionResult Subtraction([FromQuery] Subtraction operation)
        {
            if (!TryValidateModel(operation))
            {
                return BadRequest(ModelState);
            }

            var result = operation.Calculate();
            if (double.IsInfinity(result))
                return Ok("inf");
            if (double.IsNaN(result))
                return BadRequest("Nan");
            return Ok(result);
        }

        [HttpGet]
        [Route("multiplication")]
        public IActionResult Multiplication([FromQuery] Multiplication operation)
        {
            if (!TryValidateModel(operation))
            {
                return BadRequest(ModelState);
            }

            var result = operation.Calculate();
            if (double.IsInfinity(result))
                return Ok("inf");
            if (double.IsNaN(result))
                return BadRequest("Nan");
            return Ok(result);
        }
        [HttpGet]
        [Route("division")]
        public IActionResult Division([FromQuery] Division operation)
        {
            if (!TryValidateModel(operation))
            {
                return BadRequest(ModelState);
            }
            var result = operation.Calculate();
            if (double.IsInfinity(result))
                return Ok("inf");
            if (double.IsNaN(result))
                return BadRequest("Nan");
            return Ok(result);
        }

        [HttpGet]
        [Route("exponentiation")]
        public IActionResult Exponentiation([FromQuery] Exponentiation operation)
        {
            if (!TryValidateModel(operation))
            {
                return BadRequest(ModelState);
            }

            var result = operation.Calculate();
            if (double.IsInfinity(result))
                return Ok("inf");
            if (double.IsNaN(result))
                return BadRequest("Nan");
            return Ok(result);
        }

        [HttpGet]
        [Route("root")]
        public IActionResult Root([FromQuery] Root operation)
        {
            if (!TryValidateModel(operation))
                return BadRequest(ModelState);
            var result = operation.Calculate();
            if (double.IsInfinity(result))
                return Ok("inf");
            if (double.IsNaN(result))
                return BadRequest("Nan");
            return Ok(result);
        }
        [HttpGet]
        [Route("evaluate")]
        public IActionResult ExpressionEvaluation([FromQuery] ComplexEvaluation model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Stack<double> operandStack = new Stack<double>();
            List<string> orderedTokens = model.OrderTokens(model.TokenizeExpression());
            foreach (string token in orderedTokens)
            {
                double number;
                if (double.TryParse(token, out number))
                {
                    operandStack.Push(double.Parse(token));
                }
                else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "^")
                {
                    double operand2 = operandStack.Pop();
                    double operand1 = operandStack.Pop();
                    var opres = PerformOperation(token, operand1, operand2);
                    if (opres is BadRequestObjectResult)
                        return BadRequest(opres);
                    if(opres is OkObjectResult okres)
                        operandStack.Push((double)okres.Value!);
                }
            }
            var result = operandStack.Pop();
            if (double.IsInfinity(result))
                return Ok("inf");
            if (double.IsNaN(result))
                return BadRequest("Nan");
            return Ok(result);
        }
        [NonAction]
        private IActionResult PerformOperation(string operatorToken, double operand1, double operand2)
        {
            switch (operatorToken)
            {
                case "+":
                    return Addition(new Addition { firstValue = operand1, secondValue = operand2 });
                case "-":
                    return Subtraction(new Subtraction { firstValue = operand1, secondValue = operand2 });
                case "*":
                    return Multiplication(new Multiplication { firstValue = operand1, secondValue = operand2 });
                case "/":
                    return Division(new Division { firstValue = operand1, secondValue = operand2 });
                case "^":
                    return Exponentiation(new Exponentiation { firstValue = operand1, secondValue = operand2 });
                default:
                    return null;
            }
        }
    }
}