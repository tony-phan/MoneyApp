using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using money_api.Data;
using money_api.Data.Repositories;
using money_api.Mappings;
using money_api.Services;

namespace money_api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepository>();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
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
