namespace WebBackend.Server;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
<<<<<<< HEAD
<<<<<<< HEAD
=======
using Npgsql;
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
>>>>>>> 29a9675 (Implement History System)
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MatrixSerializer;
<<<<<<< HEAD
<<<<<<< HEAD
using Newtonsoft.Json;
=======
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
using Newtonsoft.Json;
>>>>>>> 29a9675 (Implement History System)

[Route("[controller]")]
[ApiController]
public class ServerController(DbSolutionContext context, IConfiguration configuration) : ControllerBase {
    private readonly JsonSerializerOptions _serializerOptions = new() { Converters = { new Array2DConverter() }};
<<<<<<< HEAD
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

<<<<<<< HEAD
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
=======
    
    // Main solver
>>>>>>> 29a9675 (Implement History System)
    [HttpPost("solve")]
    public async Task<IActionResult> Post([FromBody] SolveRequest request) {
        // Solve system
        var solution = SolveSystem(request.Matrix);
        
=======
>>>>>>> 387a279 (Add error messages and data validation)
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
<<<<<<< HEAD
            
<<<<<<< HEAD
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
=======

>>>>>>> 387a279 (Add error messages and data validation)
            // Add solution to database
>>>>>>> 29a9675 (Implement History System)
            await context.AddAsync(savedSolution);
            await context.SaveChangesAsync();
        }

<<<<<<< HEAD
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
=======
        return goodResponse ? Ok(new SolveResponse(solution)) : Ok(responseMessage);
>>>>>>> 387a279 (Add error messages and data validation)
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

<<<<<<< HEAD
        return Ok("User registered successfully");
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
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
>>>>>>> 387a279 (Add error messages and data validation)
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        // Pull client from database with given name
=======
>>>>>>> 790bda1 (implement authorization system)
=======
        // Pull client from database with given name
>>>>>>> 29a9675 (Implement History System)
        var client = await context.Clients.FirstOrDefaultAsync(c => c.clientusername == request.Username);

        if (client == null) {
            return Unauthorized("No such user found");
<<<<<<< HEAD
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
=======
>>>>>>> 790bda1 (implement authorization system)
        }
        
        // Verify given data from client
        if (!BCrypt.Net.BCrypt.Verify(request.Password.Trim(), client.clientpassword.Trim())) {
            return Unauthorized("Invalid Password or login");
        }
<<<<<<< HEAD

<<<<<<< HEAD
        // Generate JWT token
        var token = GenerateJwtToken(request.Username);
        return Ok(new { Token = token });
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
=======
        
        // Generate JWT token for client
>>>>>>> 29a9675 (Implement History System)
        var token = GenerateJwtToken(client);
        return Ok(token);
>>>>>>> 790bda1 (implement authorization system)
    }
    
    [Authorize]
    [HttpGet("history")]
    public async Task<IActionResult> GetSolutionHistory() {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 387a279 (Add error messages and data validation)
        var username = User.Identity?.Name; // Get the logged-in user's username
        if (username == null) {
            throw new NullReferenceException("No username field in token was found");
        }
<<<<<<< HEAD
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
        
=======
>>>>>>> 387a279 (Add error messages and data validation)
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
<<<<<<< HEAD
            signingCredentials: credentials);
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
            signingCredentials: credentials
        );
>>>>>>> 790bda1 (implement authorization system)

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private static double[] SolveSystem(double[][] matrix) {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 29a9675 (Implement History System)
        var matrixCopy = new double[matrix.Length, matrix[0].Length];
        for (var i = 0; i < matrix.Length; i++) {
            for (var j = 0; j < matrix[0].Length; j++) {
                matrixCopy[i, j] = matrix[i][j];
            }
        }
<<<<<<< HEAD
<<<<<<< HEAD

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
=======
        
=======

        var swapArray = new List<(int fromIndex, int toIndex)>();
>>>>>>> 387a279 (Add error messages and data validation)
        var matrixSize = matrix.Length;
>>>>>>> 29a9675 (Implement History System)
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
<<<<<<< HEAD
                    matrix[k][j] -= factor * matrix[i][j];
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
                    matrixCopy[k, j] -= factor * matrixCopy[i, j];
>>>>>>> 29a9675 (Implement History System)
                }
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
        for (var i = matrixSize - 1; i >= 0; i--) {
            result[i] = matrixCopy[i, matrixSize] / matrixCopy[i, i];
            for (var k = i - 1; k >= 0; k--) {
                matrixCopy[k, matrixSize] -= matrixCopy[k, i] * result[i];
=======
        // Back substitution to solve for variables
=======
>>>>>>> 29a9675 (Implement History System)
        for (var i = matrixSize - 1; i >= 0; i--) {
            result[i] = matrixCopy[i, matrixSize] / matrixCopy[i, i];
            for (var k = i - 1; k >= 0; k--) {
<<<<<<< HEAD
                matrix[k][matrixSize] -= matrix[k][i] * result[i];
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
                matrixCopy[k, matrixSize] -= matrixCopy[k, i] * result[i];
>>>>>>> 29a9675 (Implement History System)
            }
        }

        return result;
    }
}