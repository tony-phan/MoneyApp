using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using money_api.Data;
using money_api.Data.Repositories;
using money_api.Mappings;
using money_api.Services;

namespace money_api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration config,
        IWebHostEnvironment env)
    {
        services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepository>();

        string connectionString;
        if (env.IsDevelopment())
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }
        else
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var port = Environment.GetEnvironmentVariable("DB_PORT");

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                throw new InvalidOperationException("Database connection environment variables are not set.");

            connectionString = $"Server={host};Database={dbName};User Id={user};Password={password};Port={port}";
        }

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        // Register AutoMapper profiles
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AccountMappingProfile>();
            cfg.AddProfile<TransactionHistoryMappingProfile>();
            cfg.AddProfile<TransactionMappingProfile>();
        }, typeof(AccountMappingProfile), typeof(TransactionMappingProfile), typeof(TransactionHistoryMappingProfile));

        services.AddCors();
        return services;
    }
}
