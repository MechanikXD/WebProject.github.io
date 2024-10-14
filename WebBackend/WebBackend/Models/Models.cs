<<<<<<< HEAD
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;

=======
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
using System.ComponentModel.DataAnnotations;

>>>>>>> 790bda1 (implement authorization system)
namespace WebBackend.Models;

using Microsoft.EntityFrameworkCore;

<<<<<<< HEAD
<<<<<<< HEAD
public record SolveRequest(double[][] Matrix, string? UserToken);
public record SolveResponse(double[] Solution);

public class SavedSolutions {
    [Key]
    public int solutionid { get; set; }
    public int fkclientid { get; set; }
    public string solutionmatrix { get; set; } 
    public string solutionresult { get; set; }
    public int solutionmatrixlength { get; set; }
}
public class Client {
    [Key]
    public int clientid { get; set; }
    public string clientusername { get; set; }
    public string clientpassword { get; set; }
=======
public record SolveRequest(double[][] Matrix, int? UserId);
=======
public record SolveRequest(double[][] Matrix, string? UserToken);
>>>>>>> 790bda1 (implement authorization system)
public record SolveResponse(double[] Solution);

public class SavedSolutions {
    [Key]
    public int solutionid { get; set; }
    public int fkclientid { get; set; }
    public string solutionmatrix { get; set; } 
    public string solutionresult { get; set; }
    public int solutionmatrixlength { get; set; }
}
public class Client {
<<<<<<< HEAD
    public int ClientId { get; set; }
    public string ClientUserName { get; set; }
    public string ClientPassword { get; set; }
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
    [Key]
    public int clientid { get; set; }
    public string clientusername { get; set; }
    public string clientpassword { get; set; }
>>>>>>> 790bda1 (implement authorization system)
}

public class DbSolutionContext(DbContextOptions<DbSolutionContext> options) : DbContext(options) {
    public DbSet<SavedSolutions> Solutions { get; set; }
    public DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

<<<<<<< HEAD
<<<<<<< HEAD
        modelBuilder.Entity<SavedSolutions>().ToTable("savedsolutions", "public");
        modelBuilder.Entity<Client>().ToTable("client", "public");
=======
        // Example configuration: if your table names are different than the model names
        modelBuilder.Entity<SavedSolutions>().ToTable("SavedSolution", "public");
        modelBuilder.Entity<Client>().ToTable("Client", "public");
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
        modelBuilder.Entity<SavedSolutions>().ToTable("savedsolutions", "public");
        modelBuilder.Entity<Client>().ToTable("client", "public");
>>>>>>> 790bda1 (implement authorization system)
    }
}

public record LoginRequest(string Username, string Password);
<<<<<<< HEAD
<<<<<<< HEAD
public record RegisterRequest(string Username, string Password);
<<<<<<< HEAD
=======
public record RegisterRequest(string Username, string Password);
public record HistoryRequest(int UserId);
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
=======
public record HistoryRequest(int UserId);
public record HistoryResponse(string Matrix, string Solution);
>>>>>>> 29a9675 (Implement History System)
=======
public record RegisterRequest(string Username, string Password);
>>>>>>> 5821ea7 (Add data visualization, create histoty page)
