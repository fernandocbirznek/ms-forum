using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ms_forum.Domains;
using ms_forum.Interface;
using ms_forum.Repositories;
using System.Text;

namespace ms_forum.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<Forum>), typeof(Repository<Forum>));
            services.AddScoped(typeof(IRepository<ForumTopico>), typeof(Repository<ForumTopico>));
            services.AddScoped(typeof(IRepository<ForumTopicoReplica>), typeof(Repository<ForumTopicoReplica>));
            services.AddScoped(typeof(IRepository<ForumTopicoResposta>), typeof(Repository<ForumTopicoResposta>));
            services.AddScoped(typeof(IRepository<ForumTag>), typeof(Repository<ForumTag>));
            services.AddScoped(typeof(IRepository<ForumTopicoTag>), typeof(Repository<ForumTopicoTag>));
        }

        public static void SetupDbContext(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<ForumDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(ForumDbContext).Assembly.FullName)),
                ServiceLifetime.Transient, ServiceLifetime.Transient
                );
        }

        public static void AddAuthentication
        (
            this IServiceCollection services, 
            ConfigurationManager configuration
        )
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                GetTokenValidationParameters(configuration, options);
            });
        }

        private static void GetTokenValidationParameters
        (
            ConfigurationManager configuration,
            JwtBearerOptions jwtOptions
        )
        {
            jwtOptions.TokenValidationParameters = new()
            {
                ValidIssuer = configuration["authentication:issuer"],
                ValidAudience = configuration["authentication:audience"],
                IssuerSigningKey = GetSymmetricSecurityKey(configuration),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        }

        private static SymmetricSecurityKey GetSymmetricSecurityKey  
        ( 
            ConfigurationManager configuration 
        )
        {
            return new SymmetricSecurityKey(GetSecret(configuration));
        }

        private static byte[] GetSecret
        ( 
            ConfigurationManager configuration 
        )
        {
            return Encoding.UTF8.GetBytes(configuration.GetValue<string>("authentication:secret"));
        }
    }
}
