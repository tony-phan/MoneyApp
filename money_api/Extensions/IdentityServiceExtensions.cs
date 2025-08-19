using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using money_api.Data;
using money_api.Models;

namespace money_api.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
        }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        var jwt_key = Environment.GetEnvironmentVariable("JWT_KEY") ?? config["JwtSettings:Key"];
        if (string.IsNullOrEmpty(jwt_key))
            throw new InvalidOperationException("jwt_key variable is not set.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "sub",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["JwtSettings:Issuer"],
                ValidAudience = config["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt_key))
            };
        });

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                name: JwtBearerDefaults.AuthenticationScheme,
                securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                }
            );
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    }, new string[] {}
                }
            });
        });
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

        return services;
    }
}
