using Gateway.Api.OpenApi;
using Gateway.Api.Properties;
using Gateway.Application.AuthenticationManager;
using Gateway.Application.Encryption;
using Gateway.Application.TokenManager.Jwt;
using Gateway.Application.UserManager;
using Gateway.Infrastructure.db;
using Gateway.Infrastructure.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

main();

void main()
{
    var builder = WebApplication.CreateBuilder(args);

    Configuration.SetupConfig(builder);

    builder.Services.AddOpenApi("v1", options =>
    {
        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    });

    builder.AddServiceDefaults();
    buildInfrastructure(builder);
    buildApplication(builder);
    buildApiLevel(builder)?.Run();
}

void buildInfrastructure(WebApplicationBuilder builder, ConfigurationManager configuration)
{
    builder.Services.AddSingleton<IDbConnectionFactory>(provider =>
    {
        var configuration = provider.GetRequiredService<IConfiguration>();
        var connectionString = Configuration.GetDBConnectionString();
        return new NpgsqlDbConnectionFactory(connectionString);
    });
    builder.Services.AddScoped<IUserRepository, DbUserRepository>();
}

void buildApplication(WebApplicationBuilder builder, ConfigurationManager configuration)
{
    builder.Services.AddScoped<IPasswordEncryption, SaltAndPepperEncryption>(provider =>
    {
        var (pepperLength, pepperLetters) = Configuration.GetSaltAndPepperConfig();
        return new SaltAndPepperEncryption(pepperLetters, pepperLength);
    });
    builder.Services.AddScoped<IUserManager, UserRepositoryManager>();
    builder.Services.AddSingleton<IJwtTokenManager, SelfJwtTokenManager>(provider =>
    {
        var (issuer, audience, secretKey) = Configuration.GetJWTConfig();
        return new SelfJwtTokenManager(issuer, audience, secretKey);
    });

    builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();

}

WebApplication? buildApiLevel(WebApplicationBuilder builder)
{

    builder.Services.AddControllers();
    SetupAuthentication(builder);

    var app = builder.Build();

    app.MapDefaultEndpoints();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();

        app.UseSwagger();
        app.UseSwaggerUI(o =>
        {
            var (clientId, clientSecret) = Configuration.GetGoogleOAUTHConfig();
            o.OAuthClientId(clientId);
            o.OAuthClientSecret(clientSecret);
            o.OAuthUsePkce();
        });
        app.MapSwagger().RequireAuthorization();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    return app;
}


void SetupAuthentication(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = Configuration.GetServiceName(),
        Version = "v1",
    });
    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
    securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the Bearer Authorization : `Bearer Generated-JWT-Token`",
    });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Name = "google oauth2",
        Description = "OAuth2 authentication using Google.",
        Type = SecuritySchemeType.OAuth2,
        Flows = new()
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
                Scopes = new Dictionary<string, string>
            {
                    { "openid", "OpenID Connect scope" },
                    { "profile", "Access user profile" },
                    { "email", "Access user email" }
                    // Add additional scopes as needed for your API access
            }
            },
        }
    });
});

    builder.Services.AddAuthentication(options =>
       {
           options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
           options.DefaultForbidScheme = GoogleDefaults.AuthenticationScheme;
           options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
       }).AddCookie()
       .AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
       {
           var (clientId, clientSecret) = Configuration.GetGoogleOAUTHConfig();
           googleOptions.ClientId = clientId;
           googleOptions.ClientSecret = clientSecret;
       })
    .AddJwtBearer(options =>
       {
           var (issuer, audience, secretKey) = Configuration.GetJWTConfig();
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidIssuer = issuer,
               ValidAudience = audience,
               IssuerSigningKeys = [new SymmetricSecurityKey(Convert.FromHexString(secretKey))],
               ValidateIssuer = true,
               ValidateLifetime = true,
               ValidateAudience = true,
               ValidateIssuerSigningKey = true,

           };
       });

    builder.Services.AddAuthorization();
}