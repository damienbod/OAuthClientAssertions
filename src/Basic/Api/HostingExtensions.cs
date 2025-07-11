using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using NetEscapades.AspNetCore.SecurityHeaders.Infrastructure;
using Serilog;

namespace Api;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        var deploySwaggerUI = builder.Environment.IsDevelopment();

        builder.Services.AddSecurityHeaderPolicies()
        .SetPolicySelector((PolicySelectorContext ctx) =>
        {
            // sum is weak security headers due to Swagger UI deployment
            // should only use in development
            if (deploySwaggerUI)
            {
                // Weakened security headers for Swagger UI
                if (ctx.HttpContext.Request.Path.StartsWithSegments("/swagger"))
                {
                    return SecurityHeadersDefinitionsSwagger.GetHeaderPolicyCollection(builder.Environment.IsDevelopment());
                }

                // Strict security headers
                return SecurityHeadersDefinitionsAPI.GetHeaderPolicyCollection(builder.Environment.IsDevelopment());
            }
            // Strict security headers for production
            else
            {
                return SecurityHeadersDefinitionsAPI.GetHeaderPolicyCollection(builder.Environment.IsDevelopment());
            }
        });

        var stsServer = configuration["StsServer"];

        services.AddAuthentication("bearer")
            .AddJwtBearer("bearer", options =>
            {
                options.Authority = stsServer;
                options.TokenValidationParameters.ValidateAudience = false;
                options.MapInboundClaims = false;

                options.TokenValidationParameters.ValidTypes = ["at+jwt"];
            });

        services.AddAuthorization(options =>
            options.AddPolicy("protectedScope", policy =>
            {
                policy.RequireClaim("scope", "mobile");
            })
        );

        services.AddSwaggerGen(c =>
        {
            // add JWT Authentication
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // must be lower case
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, Array.Empty<string>()}
            });

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Basic API",
                Version = "v1",
                Description = "Application API",
                Contact = new OpenApiContact
                {
                    Name = "damienbod",
                    Email = string.Empty,
                    Url = new Uri("https://damienbod.com/"),
                },
            });
        });

        services.AddControllers();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        IdentityModelEventSource.ShowPII = true;
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSecurityHeaders();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            });
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers()
            .RequireAuthorization();

        return app;
    }
}