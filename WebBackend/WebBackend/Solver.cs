namespace WebBackend.Controllers;

using Microsoft.AspNetCore.Mvc;
using Context;

[ApiController]
[Route("solver")]
public class SolverController(SolutionContext context) : ControllerBase {
    // POST: /solver/solve
    [HttpPost("solve")]
    public async Task<IActionResult> Solve([FromBody] double[,] matrix) {
        if (matrix.GetLength(0) == 0) {
            return BadRequest("Matrix cannot be null or empty");
        }

        try {
            // Solve the system of linear equations
            var result = SolveSystem(matrix);

            // Generate a unique ID for the result
            var resultId = Guid.NewGuid();

            // Create a row for database to store
            var savedSolution = new SavedSolution {
                SolutionId = resultId,
                UserId = 1, // PLACEHOLDER
                Matrix = SavedSolution.MatrixToString(matrix),
                Result = SavedSolution.ResultToString(result),
                MatrixLength = result.Length
            };

            context.SavedSolutions.Add(savedSolution);
            await context.SaveChangesAsync();

            // Return the ID to the client
            return Ok(new { id = resultId });
        }
        catch (Exception ex) {
            return BadRequest(new { message = "Error solving equations", details = ex.Message });
        }

        // GET: /solver/result/{id}
        [HttpGet("result/{id}")]
        async Task<IActionResult> GetResult(Guid id) {
            // Retrieve the result from the database by SolutionId
            var solution = await context.SavedSolutions.FindAsync(CancellationToken.None, id);

            if (solution != null) {
                return Ok(new { matrix = solution.Matrix, result = solution.Result });
            }

            return NotFound("Result not found");
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