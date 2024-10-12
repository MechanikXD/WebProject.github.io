using System.ComponentModel.DataAnnotations;

namespace WebBackend.Models;

using Microsoft.EntityFrameworkCore;

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
}

public class DbSolutionContext(DbContextOptions<DbSolutionContext> options) : DbContext(options) {
    public DbSet<SavedSolutions> Solutions { get; set; }
    public DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SavedSolutions>().ToTable("savedsolutions", "public");
        modelBuilder.Entity<Client>().ToTable("client", "public");
    }
}

public record LoginRequest(string Username, string Password);
public record RegisterRequest(string Username, string Password);
public record HistoryRequest(int UserId);
public record HistoryResponse(string Matrix, string Solution);