using System.Runtime.CompilerServices;
using Microsoft.OpenApi.Models;

namespace HubtelWalletAPI.Swagger
{
    public static class SwaggerUI
    {
        public static IServiceCollection ExtendedSwaggerGen(this IServiceCollection service)
        {
            service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Hubtel Wallet", Version = "v1" });

                // Add basic authentication security definition
                c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                {
                    Description = "Basic authentication header",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic"
                });

                // Add basic authentication security requirement
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Basic"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return service;
        }
    }
}
