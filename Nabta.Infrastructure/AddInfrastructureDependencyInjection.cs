using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Nabta.Infrastructure.Data;
using Nabta.Infrastructure.identity;
using System.Text;
using Nabta.Infrastructure.Implementations;
using Nabta.Application.Interfaces.Services;
using Nabta.Infrastructure.implementations;
using Nabta.Infrastructure.Helpers;

namespace Infrastructure
{
    public static class AddInfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            var databaseUrl = configuration["DATABASE_URL"]
                ?? configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(ToNpgsqlConnectionString(databaseUrl)));

            // JWT Options
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            // Frontend Options
            services.Configure<FrontendOptions>(configuration.GetSection("Frontend"));

            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtOptions>>().Value);

            // Identity + EF Core
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                configuration["Jwt:Key"]!))
                    };
            });

            // Application Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }

        private static string ToNpgsqlConnectionString(string? databaseUrl)
        {
            if (string.IsNullOrWhiteSpace(databaseUrl)
                || !databaseUrl.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
            {
                return databaseUrl ?? throw new InvalidOperationException(
                    "DATABASE_URL or ConnectionStrings:DefaultConnection must be configured.");
            }

            var uri = new Uri(databaseUrl);
            var credentials = uri.UserInfo.Split(':', 2);

            if (credentials.Length != 2)
            {
                throw new InvalidOperationException("DATABASE_URL must include a username and password.");
            }

            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = uri.Host,
                Port = uri.IsDefaultPort ? 5432 : uri.Port,
                Database = uri.AbsolutePath.Trim('/'),
                Username = Uri.UnescapeDataString(credentials[0]),
                Password = Uri.UnescapeDataString(credentials[1]),
                SslMode = SslMode.Require
            };

            return connectionString.ConnectionString;
        }
    }
}
