namespace WebBackend.Models;

using Microsoft.EntityFrameworkCore;

public record SolveRequest(double[][] Matrix, int? UserId);
public record SolveResponse(double[] Solution);

public class SavedSolutions {
    private int SolutionId { get; set; }
    public int FkClientId { get; set; }
    public string SolutionMatrix { get; set; } 
    public string SolutionResult { get; set; }
    public int SolutionMatrixLength { get; set; }
}

public class Client {
    public int ClientId { get; set; }
    public string ClientUserName { get; set; }
    public string ClientPassword { get; set; }
}

public class DbSolutionContext(DbContextOptions<DbSolutionContext> options) : DbContext(options) {
    public DbSet<SavedSolutions> Solutions { get; set; }
    public DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // Example configuration: if your table names are different than the model names
        modelBuilder.Entity<SavedSolutions>().ToTable("SavedSolution", "public");
        modelBuilder.Entity<Client>().ToTable("Client", "public");
    }
}

public record LoginRequest(string Username, string Password);
public record RegisterRequest(string Username, string Password);
public record HistoryRequest(int UserId);