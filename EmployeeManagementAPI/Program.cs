using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Services;
using EmployeeManagementAPI.Middleware;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
namespace EmployeeManagementAPI
{
    public class Program
    {
        public static void Main(string[] args)

        {
            Log.Logger = new LoggerConfiguration()
       //.ReadFrom.Configuration(builder.Configuration)
       .WriteTo.Console(outputTemplate:
           "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
       .WriteTo.File("Logs/app-log-.txt",
           rollingInterval: RollingInterval.Day,
           outputTemplate:
           "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
       .CreateLogger();

            try
            {
                Log.Information("Starting EmployeeManagementAPI....");

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

                // Add services to the container.
                builder.Services.AddControllers();
                builder.Services.AddScoped<IEmployeeService, EmployeeService>();
                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                // test change
                builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));
                var jwtSettings = builder.Configuration.GetSection("JwtSettings");

                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtSettings["Issuer"],
                            ValidAudience = jwtSettings["Audience"],

                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
                        };
                    });

                builder.Services.AddAuthorization();
    

                var app = builder.Build();

                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    SeedData.Initialize(context);
                }

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseMiddleware<RequestLoggingMiddleware>();

                app.UseHttpsRedirection();

                app.UseAuthentication();  // must come before UseAuthorization
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {

                Log.Fatal(ex, "Application failed to start");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
