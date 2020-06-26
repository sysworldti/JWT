using JWT.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
namespace JWT.Extensions
{
    /// <summary>
    /// SwaggerExtensions
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "JWT Authentication",
                    Version = "Versão 1.0",
                    Description = "API JWT Authentication",
                    Contact = new OpenApiContact
                    {
                        Name = "JWT",
                        Url = new Uri("https://www.linkedin.com/in/sysworldti/"),
                        Email = "ricardompb@outlook.com"
                    }
                });

                c.OperationFilter<AuthResponsesOperationFilter>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Autenticação para obter dados da JWT Authentication...",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, new string[] {}
                    }
                });

                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{PlatformServices.Default.Application.ApplicationName}.xml"));
            });
        }

        /// <summary>
        /// SwaggerConfigure
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint(
#if DEBUG
                    "/swagger/v1/swagger.json"
#else
                    "/webapi/swagger/v1/swagger.json"
#endif
                , "JWT Ricardo"));
        }
    }
}
