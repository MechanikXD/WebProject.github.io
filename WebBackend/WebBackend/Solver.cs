using Microsoft.AspNetCore.Mvc;

namespace WebBackend.Controllers {
    [ApiController]
    [Route("solver")]
    public class SolverController : ControllerBase {
        // POST: /solver/solve
        [HttpPost("solve")]
        public IActionResult Solve([FromBody] double[,] matrix) {
            try {
                // Perform Gaussian Elimination to solve the system of equations
                var result = SolveSystem(matrix);
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest(new { message = "Error solving equations", details = ex.Message });
            }
        }

        private double[] SolveSystem(double[,] matrix) {
            var matrixSize = matrix.GetLength(0);
            var result = new double[matrixSize];

            for (var i = 0; i < matrixSize; i++) {
                // Make the diagonal contain all 1's
                for (var k = i + 1; k < matrixSize; k++) {
                    var factor = matrix[k, i] / matrix[i, i];
                    for (var j = i; j <= matrixSize; j++) {
                        matrix[k, j] -= factor * matrix[i, j];
                    }
                }
            }

            // Back substitution to solve for variables
            for (var i = matrixSize - 1; i >= 0; i--) {
                result[i] = matrix[i, matrixSize] / matrix[i, i];
                for (var k = i - 1; k >= 0; k--) {
                    matrix[k, matrixSize] -= matrix[k, i] * result[i];
                }
            }

            return result;
        }
    }
}