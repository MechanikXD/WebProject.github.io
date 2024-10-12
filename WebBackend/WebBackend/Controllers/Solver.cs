namespace WebBackend.Server;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MatrixSerializer;
using Newtonsoft.Json;

[Route("[controller]")]
[ApiController]
public class ServerController(DbSolutionContext context, IConfiguration configuration) : ControllerBase {
    private readonly JsonSerializerOptions _serializerOptions = new() { Converters = { new Array2DConverter() }};
    
    // Main solver
    [HttpPost("solve")]
    public async Task<IActionResult> Post([FromBody] SolveRequest request) {
        // Solve system
        var solution = SolveSystem(request.Matrix);
        
        // If user has token (logged in) - add this solution to database (as history)
        if (request.UserToken != null) {
            // Decodes the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(request.UserToken); 

            // Extract the user ID from the token's claims
            var username = jwtToken?.Claims
                .FirstOrDefault(c => c.Type == "sub")?.Value.Trim();
            var client = await context.Clients.FirstOrDefaultAsync(c => c.clientusername == username);
            if (client == null) {
                throw new Exception("No client was found in database but token was provided");
            }
            
            // Create solution to store in database
            var savedSolution = new SavedSolutions {
                fkclientid = client.clientid,
                solutionmatrix = System.Text.Json.JsonSerializer.Serialize(request.Matrix, _serializerOptions),
                solutionresult = System.Text.Json.JsonSerializer.Serialize(solution),
                solutionmatrixlength = request.Matrix.Length
            };
            
            // Add solution to database
            await context.AddAsync(savedSolution);
            await context.SaveChangesAsync();
        }

        return Ok(new SolveResponse(solution));
    }
    
    // Debug task, never actually used in frontend
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
        // Hash password for safety
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var newUser = new Client { clientusername = request.Username, clientpassword = passwordHash };
        // Add user into database
        await context.AddAsync(newUser);
        await context.SaveChangesAsync();

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) {
        // Pull client from database with given name
        var client = await context.Clients.FirstOrDefaultAsync(c => c.clientusername == request.Username);

        if (client == null) {
            return Unauthorized("No such user found");
        }
        
        // Verify given data from client
        if (!BCrypt.Net.BCrypt.Verify(request.Password.Trim(), client.clientpassword.Trim())) {
            return Unauthorized("Invalid Password or login");
        }
        
        // Generate JWT token for client
        var token = GenerateJwtToken(client);
        return Ok(token);
    }
    
    [Authorize]
    [HttpGet("history")]
    public async Task<IActionResult> GetSolutionHistory() {
        var username = User.Identity.Name; // Get the logged-in user's username
        
        var userId = await context.Clients.FirstOrDefaultAsync(c => c.clientusername.Trim() == username.Trim());
        // var matrixArray = new List<HistoryResponse>();
        // Store all entries where user index matches foreign key
        await context.Solutions.LoadAsync();
        var matrixArray = context.Solutions.Local.Where(solutions => solutions.fkclientid == userId.clientid);

        return Ok(JsonConvert.SerializeObject(matrixArray));
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteSolution([FromBody] int relativeId) {
        var username = User.Identity.Name; // Get the logged-in user's username
        
        // Since index is relative we need user history to determent correct index in database
        var userId = await context.Clients.FirstOrDefaultAsync(c => c.clientusername.Trim() == username.Trim());
        
        await context.Solutions.LoadAsync();
        var userSolutions = context.Solutions.Local.Where(source => source.fkclientid == userId.clientid).ToList();
        // Query request
        context.Solutions.Remove(userSolutions[relativeId]);
        await context.SaveChangesAsync();
        
        return Ok("Entry deleted");
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
        var matrixCopy = new double[matrix.Length, matrix[0].Length];
        for (var i = 0; i < matrix.Length; i++) {
            for (var j = 0; j < matrix[0].Length; j++) {
                matrixCopy[i, j] = matrix[i][j];
            }
        }
        
        var matrixSize = matrix.Length;
        var result = new double[matrixSize];

        for (var i = 0; i < matrixSize; i++) {
            for (var k = i + 1; k < matrixSize; k++) {
                var factor = matrixCopy[k, i] / matrixCopy[i, i];
                for (var j = i; j <= matrixSize; j++) {
                    matrixCopy[k, j] -= factor * matrixCopy[i, j];
                }
            }
        }

        for (var i = matrixSize - 1; i >= 0; i--) {
            result[i] = matrixCopy[i, matrixSize] / matrixCopy[i, i];
            for (var k = i - 1; k >= 0; k--) {
                matrixCopy[k, matrixSize] -= matrixCopy[k, i] * result[i];
            }
        }

        return result;
    }
}