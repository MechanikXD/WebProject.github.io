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

        if (request.UserToken != null) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(request.UserToken); // Decodes the token

            // Extract the user ID from the token's claims
            var username = jwtToken?.Claims
                .FirstOrDefault(c => c.Type == "sub")?.Value.Trim();
            var client = await context.Clients.FirstOrDefaultAsync(c => c.clientusername == username);
            if (client == null) {
                throw new Exception("No client was found in database but token was provided");
            }
            
            var savedSolution = new SavedSolutions {
                fkclientid = client.clientid,
                solutionmatrix = JsonSerializer.Serialize(request.Matrix, _serializerOptions),
                solutionresult = JsonSerializer.Serialize(solution),
                solutionmatrixlength = request.Matrix.Length
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
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var newUser = new Client { clientusername = request.Username, clientpassword = passwordHash };
        await context.AddAsync(newUser);
        await context.SaveChangesAsync();

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) {
        var client = await context.Clients.FirstOrDefaultAsync(c => c.clientusername == request.Username);

        if (client == null) {
            return Unauthorized("No such user found");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password.Trim(), client.clientpassword.Trim())) {
            return Unauthorized("Invalid Password or login");
        }

        var token = GenerateJwtToken(client);
        return Ok(token);
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

    private string GenerateJwtToken(Client client) {
        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, client.clientusername),
            new Claim(JwtRegisteredClaimNames.Jti, client.clientid.ToString()),
            new Claim(ClaimTypes.Name, client.clientusername)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials
        );

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