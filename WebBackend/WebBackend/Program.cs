<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
﻿namespace WebBackend;

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Models;

public class Startup(IConfiguration configuration) {
    private IConfiguration Configuration { get; } = configuration;

    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options => {
            options.AddPolicy("AllowAll", b => {
                b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
        
        builder.Services.AddLogging();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<DbSolutionContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };
            });

        builder.Services.AddAuthorization();
        
        var app = builder.Build();
        
        app.UseCors("AllowAll");
        if (app.Environment.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.Run();
    }
    
    public void ConfigureServices(IServiceCollection services) {
        services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
                };
            });
        services.AddAuthorization();
        services.AddDbContext<DbSolutionContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        
        services.AddCors(options => {
            options.AddPolicy("AllowAll",
                builder => {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
=======
﻿namespace WebBackend;
=======
﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)

namespace WebBackend;
=======
﻿namespace WebBackend;
>>>>>>> 29a9675 (Implement History System)

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Models;

public class Startup(IConfiguration configuration) {
    private IConfiguration Configuration { get; } = configuration;

    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options => {
            options.AddPolicy("AllowAll", b => {
                b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
        
        builder.Services.AddLogging();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<DbSolutionContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
<<<<<<< HEAD
        var app = builder.Build();
        
        app.UseCors("AllowAll");
        if (app.Environment.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }

        app.MapControllers();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.Run();
    }
    
    public void ConfigureServices(IServiceCollection services) {
        services.AddDbContext<DbSolutionContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
<<<<<<< HEAD
                        .AllowAnyMethod()
                        .AllowAnyHeader();
>>>>>>> ea3faf1 (Create Config and Startup methods to run servers)
=======
                           .AllowAnyMethod()
                           .AllowAnyHeader();
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
                });
        });
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
=======
        builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
>>>>>>> 29a9675 (Implement History System)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };
            });

        builder.Services.AddAuthorization();
        
        var app = builder.Build();
        
        app.UseCors("AllowAll");
        if (app.Environment.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.Run();
    }
    
    public void ConfigureServices(IServiceCollection services) {
        services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
                };
            });
        services.AddAuthorization();
        services.AddDbContext<DbSolutionContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        
        services.AddCors(options => {
            options.AddPolicy("AllowAll",
                builder => {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
        services.AddControllers();
    }

<<<<<<< HEAD
<<<<<<< HEAD
=======
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

>>>>>>> ea3faf1 (Create Config and Startup methods to run servers)
=======
>>>>>>> 758e2aa (Rearenge Project. Implement Entity Framework functional. Make server actually respond to requests)
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        else {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
<<<<<<< HEAD
<<<<<<< HEAD
        app.UseRouting();
        app.UseCors("AllowAll");
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
=======
﻿
>>>>>>> 7210146 (Create empty backend project)
=======
=======
        app.UseRouting();
>>>>>>> 29a9675 (Implement History System)
        app.UseCors("AllowAll");
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
>>>>>>> ea3faf1 (Create Config and Startup methods to run servers)
