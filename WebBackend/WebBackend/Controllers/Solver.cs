namespace WebBackend.Server;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
<<<<<<< HEAD
=======
using Npgsql;
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MatrixSerializer;
<<<<<<< HEAD
using Newtonsoft.Json;
=======
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)

[Route("[controller]")]
[ApiController]
public class ServerController(DbSolutionContext context, IConfiguration configuration) : ControllerBase {
    private readonly JsonSerializerOptions _serializerOptions = new() { Converters = { new Array2DConverter() }};
<<<<<<< HEAD
    
    // Main solver
    [HttpPost("solve")]
    public async Task<IActionResult> Post([FromBody] SolveRequest request) {
        // Solve system
        var goodResponse = true;
        var responseMessage = "";
        var solution = Array.Empty<double>();
        try {
            var validationResult = IsValidMatrix(request.Matrix);
            if (validationResult.isValid) {
                solution = SolveSystem(request.Matrix);
                if (solution.Length == 0) {
                    goodResponse = false;
                    responseMessage = "Matrix don't save single solution";
                }
            }
            else {
                responseMessage = validationResult.message;
                goodResponse = false;
            }
        }
        catch (Exception error) {
            responseMessage = error.Message;
            goodResponse = false;
        }

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
            await context.Solutions.LoadAsync();
            var clientSolutionsCount =
                context.Solutions.Local.Count(clientSolutions => clientSolutions.fkclientid == client.clientid);
            if (clientSolutionsCount > 100) {
                responseMessage = "Too many solutions are stored, please clean your history";
                goodResponse = false;
            }

            // Create solution to store in database
            var savedSolution = new SavedSolutions {
                fkclientid = client.clientid,
                solutionmatrix = System.Text.Json.JsonSerializer.Serialize(request.Matrix, _serializerOptions),
                solutionresult = System.Text.Json.JsonSerializer.Serialize(solution),
                solutionmatrixlength = request.Matrix.Length
            };

            // Add solution to database
=======
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
            
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
            await context.AddAsync(savedSolution);
            await context.SaveChangesAsync();
        }

<<<<<<< HEAD
        return goodResponse ? Ok(new SolveResponse(solution)) : Ok(responseMessage);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request) {
        try {
            // Hash password for safety
            await context.Clients.LoadAsync();
            if (await context.Clients.FirstOrDefaultAsync(client => client.clientusername == request.Username) !=
                null) {
                throw new NullReferenceException("User with the same name already exists");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new Client { clientusername = request.Username, clientpassword = passwordHash };
            // Add user into database
            await context.AddAsync(newUser);
            await context.SaveChangesAsync();

            return Ok("User registered successfully");
        }
        catch (Exception error) {
            return Ok(error.Message);
        }
=======
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
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) {
<<<<<<< HEAD
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
=======
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
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
    }
    
    [Authorize]
    [HttpGet("history")]
    public async Task<IActionResult> GetSolutionHistory() {
<<<<<<< HEAD
        var username = User.Identity?.Name; // Get the logged-in user's username
        if (username == null) {
            throw new NullReferenceException("No username field in token was found");
        }
        var userId = await context.Clients.FirstOrDefaultAsync(c => c.clientusername.Trim() == username.Trim());

        if (userId == null) {
            throw new NullReferenceException("No user with such username was found");
        }
        // Store all entries where user index matches foreign key
        await context.Solutions.LoadAsync();
        var matrixArray = context.Solutions.Local.Where(solutions => solutions.fkclientid == userId.clientid).ToArray();

        return Ok(JsonConvert.SerializeObject(matrixArray));
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteSolution([FromBody] int solutionid) {
        // Query request
        context.Solutions.Remove(await context.Solutions.FirstOrDefaultAsync(solutions => solutions.solutionid == solutionid) 
                                 ?? throw new NullReferenceException("No solution with such ID was found"));
        await context.SaveChangesAsync();
        
        return Ok("Entry deleted");
    }

    private (bool isValid, string message) IsValidMatrix(double[][] matrix) {
        var reason = "Matrix is valid";
        var allValuesAreValid = true;
        if (matrix.Length + 1 == matrix[0].Length && matrix.Length is > 1 and < 11) {
            foreach (var t in matrix) {
                for (var j = 0; j < matrix[0].Length; j++) {
                    if (!double.IsNaN(t[j])) {
                        continue;
                    }

                    allValuesAreValid = false;
                    reason = "One of the cell contain NaN";
                    break;
                }

                if (!allValuesAreValid) {
                    break;
                }
            }
        }
        else {
            allValuesAreValid = false;
            reason = "Bad matrix size";
        }

        return (allValuesAreValid, reason);
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
=======
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
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private static double[] SolveSystem(double[][] matrix) {
<<<<<<< HEAD
        var matrixCopy = new double[matrix.Length, matrix[0].Length];
        for (var i = 0; i < matrix.Length; i++) {
            for (var j = 0; j < matrix[0].Length; j++) {
                matrixCopy[i, j] = matrix[i][j];
            }
        }

        var swapArray = new List<(int fromIndex, int toIndex)>();
        var matrixSize = matrix.Length;
        var result = new double[matrixSize];

        for (var i = 0; i < matrixSize; i++) {
            if (matrixCopy[i, i] == 0) {
                var rowsWereSwapped = false;
                for (var k = i + 1; k < matrixSize; k++) {
                    if (matrixCopy[i, k] == 0) {
                        continue;
                    }

                    for (var j = 0; j < matrixSize + 1; j++) {
                        (matrixCopy[i, j], matrixCopy[k, j]) = (matrixCopy[k, j], matrixCopy[i, j]);
                    }
                    swapArray.Add((i, k));
                    rowsWereSwapped = true;
                    break;
                }

                if (!rowsWereSwapped) {
                    return Array.Empty<double>();
                }
            }
            
            for (var k = i + 1; k < matrixSize; k++) {
                var factor = matrixCopy[k, i] / matrixCopy[i, i];
                for (var j = i; j <= matrixSize; j++) {
                    matrixCopy[k, j] -= factor * matrixCopy[i, j];
=======
        var matrixSize = matrix.GetLength(0);
        var result = new double[matrixSize];

        for (var i = 0; i < matrixSize; i++) {
            // Make the diagonal contain all 1's
            for (var k = i + 1; k < matrixSize; k++) {
                var factor = matrix[k][i] / matrix[i][i];
                for (var j = i; j <= matrixSize; j++) {
                    matrix[k][j] -= factor * matrix[i][j];
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
                }
            }
        }

<<<<<<< HEAD
        for (var i = matrixSize - 1; i >= 0; i--) {
            result[i] = matrixCopy[i, matrixSize] / matrixCopy[i, i];
            for (var k = i - 1; k >= 0; k--) {
                matrixCopy[k, matrixSize] -= matrixCopy[k, i] * result[i];
=======
        // Back substitution to solve for variables
        for (var i = matrixSize - 1; i >= 0; i--) {
            result[i] = matrix[i][matrixSize] / matrix[i][i];
            for (var k = i - 1; k >= 0; k--) {
                matrix[k][matrixSize] -= matrix[k][i] * result[i];
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
            }
        }

        return result;
    }
}