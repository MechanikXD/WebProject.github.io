namespace WebBackend;

using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Context;
using Balancer;

public class Startup(IConfiguration configuration) {
    public IConfiguration Configuration { get; } = configuration;

    public static void Main(string[] args) {
        ConfigureServices(new ServiceCollection());
        var loadBalancer = new LoadBalancer();
        loadBalancer.Run();
        CreateHostBuilder(["5001", "5002", "5003"]).Build().Run();
    }
    
    public static void ConfigureServices(IServiceCollection services) {
        const string dbConnectionString = "Host=localhost;Port=5432;Database=WebProject;Username=postgres;Password=qwsdcvgtrfmlg;";

        services.AddDbContext<SolutionContext>(options => options.UseNpgsql(new SqlConnection(dbConnectionString)));
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        services.AddControllers();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        else {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}