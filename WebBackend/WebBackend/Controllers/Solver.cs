namespace WebBackend.Server;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MatrixSerializer;

[Route("[controller]")]
[ApiController]
public class ServerController(DbSolutionContext context, IConfiguration configuration) : ControllerBase {
    private readonly JsonSerializerOptions _serializerOptions = new() { Converters = { new Array2DConverter() }};
    [HttpPost("solve")]
    public async Task<IActionResult> Post([FromBody] SolveRequest request) {
        var solution = SolveSystem(request.Matrix);

        if (request.UserId != null) {
            var savedSolution = new SavedSolutions {
                FkClientId = (int)request.UserId,
                SolutionMatrix = JsonSerializer.Serialize(request.Matrix, _serializerOptions),
                SolutionResult = JsonSerializer.Serialize(solution),
                SolutionMatrixLength = request.Matrix.Length
            };
            
            await context.AddAsync(savedSolution);
            await context.SaveChangesAsync();
        }

        return Ok(new SolveResponse(solution));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSolution(int id) {
        Console.WriteLine($"Received GET request for {id}id");
        var solution = await context.FindAsync(typeof(SavedSolutions), id);

        if (solution == null)
            return NotFound();
        solution = (SavedSolutions)solution;

        return Ok(solution);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request) {
        await using (var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))) {
            connection.Open();
            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            const string query = "INSERT INTO \"Users\" (username, password_hash) VALUES (@username, @password_hash)";

            await using (var cmd = new NpgsqlCommand(query, connection)) {
                cmd.Parameters.AddWithValue("username", request.Username);
                cmd.Parameters.AddWithValue("password_hash", passwordHash);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) {
        string? passwordHash = null;
        await using (var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))) {
            connection.Open();
            const string query = "SELECT password_hash FROM \"Users\" WHERE username = @username";
            await using (var cmd = new NpgsqlCommand(query, connection)) {
                cmd.Parameters.AddWithValue("username", request.Username);
                var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync()) {
                    passwordHash = reader.GetString(0);
                }
            }
        }

        // Verify password
        if (passwordHash == null || !BCrypt.Net.BCrypt.Verify(request.Password, passwordHash)) {
            return Unauthorized("Invalid credentials");
        }

        // Generate JWT token
        var token = GenerateJwtToken(request.Username);
        return Ok(new { Token = token });
    }
    
    [Authorize]
    [HttpGet("history")]
    public async Task<IActionResult> GetSolutionHistory() {
        var username = User.Identity.Name; // Get the logged-in user's username

        // Query to get the user ID
        int userId;
        await using (var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))) {
            connection.Open();
            const string query = "SELECT user_id FROM \"Users\" WHERE username = @username";
            await using (var cmd = new NpgsqlCommand(query, connection)) {
                cmd.Parameters.AddWithValue("username", username);
                userId = (int)await cmd.ExecuteScalarAsync();
            }
        }

        // Query to get user's solutions
        var solutions = new List<string>();
        await using (var connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))) {
            connection.Open();
            const string query = "SELECT solution FROM \"SavedSolutions\" WHERE user_id = @user_id";
            await using (var cmd = new NpgsqlCommand(query, connection)) {
                cmd.Parameters.AddWithValue("user_id", userId);
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync()) {
                    solutions.Add(reader.GetString(0));
                }
            }
        }

        return Ok(solutions);
    }

    private string GenerateJwtToken(string username) {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private static double[] SolveSystem(double[][] matrix) {
        var matrixSize = matrix.GetLength(0);
        var result = new double[matrixSize];

        for (var i = 0; i < matrixSize; i++) {
            // Make the diagonal contain all 1's
            for (var k = i + 1; k < matrixSize; k++) {
                var factor = matrix[k][i] / matrix[i][i];
                for (var j = i; j <= matrixSize; j++) {
                    matrix[k][j] -= factor * matrix[i][j];
                }
            }
        }

        // Back substitution to solve for variables
        for (var i = matrixSize - 1; i >= 0; i--) {
            result[i] = matrix[i][matrixSize] / matrix[i][i];
            for (var k = i - 1; k >= 0; k--) {
                matrix[k][matrixSize] -= matrix[k][i] * result[i];
            }
        }

        return result;
    }
}