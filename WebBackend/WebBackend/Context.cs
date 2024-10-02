namespace WebBackend.Context;

using System.Text;
using Microsoft.EntityFrameworkCore;

public class SolutionContext(DbContextOptions<SolutionContext> options) : DbContext(options) {
    public DbSet<SavedSolution> SavedSolutions { get; init; }
}

public class SavedSolution {
    public int UserId { get; set; }
    public Guid SolutionId { get; set; }
    public string Matrix { get; set; }
    public string Result { get; set; }
    public int MatrixLength { get; set; }

    public static string MatrixToString(double[,] matrix) {
        var stringBuilder = new StringBuilder();

        for (var i = 0; i < matrix.Length; i++) {
            for (var j = 0; j < matrix.Length + 1; j++) {
                stringBuilder.Append($"{matrix[i, j]}, ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }

    public static string ResultToString(IEnumerable<double> result) {
        var stringBuilder = new StringBuilder();

        foreach (var t in result) {
            stringBuilder.Append($"{t}, ");
        }

        stringBuilder.Remove(stringBuilder.Length - 2, 2);
        stringBuilder.AppendLine();

        return stringBuilder.ToString();
    }
}