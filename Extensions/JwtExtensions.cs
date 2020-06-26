using JWT.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JWT.Extensions
{
    /// <summary>
    /// JwtExtensions
    /// </summary>
    public static class JwtExtensions
    {
        /// <summary>
        /// ConfigureJwt
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(settings);
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Get<AppSettings>().ApiKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        /// <summary>
        /// UseJwt
        /// </summary>
        /// <param name="app"></param>
        public static void UseJwt(this IApplicationBuilder app)
        {
            app.UseAuthentication();
        }
    }
}