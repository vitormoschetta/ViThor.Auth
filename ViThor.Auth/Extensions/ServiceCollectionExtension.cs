using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ViThor.Auth.Services.Email;
using ViThor.Auth.Services.Jwt;
using ViThor.Auth.Settings;

namespace ViThor.Auth.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void BuildViThorSettings(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            
            builder.Services.Configure<ViThorAuthSettings>(builder.Configuration);

            var viThorAuthSettings = builder.Services.BuildServiceProvider()?.GetService<IOptions<ViThorAuthSettings>>()?.Value ?? throw new ArgumentNullException(nameof(ViThorAuthSettings));

            if (viThorAuthSettings.JwtConfig == null)
                throw new ArgumentNullException($"{nameof(ViThorAuthSettings.JwtConfig)} is null. Please check your appsettings.json file.");

            SwaggerConfig(builder.Services);
            AuthenticationConfig(builder.Services);
        }       

        private static void SwaggerConfig(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "ViThor.Auth", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"
                        Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                            {
                        {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                            });
            });
        }


        private static void AuthenticationConfig(IServiceCollection services)
        {
            var appSettings = services?.BuildServiceProvider()?.GetService<IOptions<ViThorAuthSettings>>()?.Value ?? throw new ArgumentNullException(nameof(ViThorAuthSettings));

            var jwtKey = appSettings.JwtConfig.Secret;
            var key = Encoding.ASCII.GetBytes(jwtKey);
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = appSettings.JwtConfig.RequireHttpsMetadata;
                    options.SaveToken = appSettings.JwtConfig.SaveToken;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuerSigningKey = appSettings.JwtConfig.ValidateIssuerSigningKey,
                        ValidateLifetime = appSettings.JwtConfig.ValidateLifetime,
                        ValidateIssuer = appSettings.JwtConfig.ValidateIssuer,
                        ValidIssuer = appSettings.JwtConfig.Issuer,
                        ValidateAudience = appSettings.JwtConfig.ValidateAudience,
                        ValidAudience = appSettings.JwtConfig.Audience,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}