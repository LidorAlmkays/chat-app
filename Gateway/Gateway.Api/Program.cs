using Gateway.Api.OpenApi;
using Gateway.Application.AuthenticationManager;
using Gateway.Application.Encryption;
using Gateway.Application.TokenManager.Jwt;
using Gateway.Application.UserManager;
using Gateway.Infrastructure.db;
using Gateway.Infrastructure.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

main();

void main()
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
    builder.Services.AddSingleton<IConfiguration>(configuration);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        var (secretKey, issuer, audience) = extractJwtSettings(configuration);
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
        };
    });

    builder.Services.AddAuthorization();
    builder.Services.AddOpenApi("v1", options =>
    {
        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    });
    builder.AddServiceDefaults();
    buildInfrastructure(builder, configuration);
    buildApplication(builder, configuration);
    var app = buildApiLevel(builder, configuration);
    app?.Run();
}

void buildInfrastructure(WebApplicationBuilder builder, ConfigurationManager configuration)
{
    var connectionString = configuration.GetValue<string>("DbConnectionString") ?? throw new InvalidOperationException("DbConnectionString configuration is missing.");
    ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString);
    builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new NpgsqlDbConnectionFactory(connectionString));
    builder.Services.AddScoped<IUserRepository, DbUserRepository>();
}

void buildApplication(WebApplicationBuilder builder, ConfigurationManager configuration)
{
    builder.Services.AddScoped<IPasswordEncryption, SaltAndPepperEncryption>(provider =>
    {
        var pepperLetters = configuration.GetValue<string>("pepperLetters") ?? throw new InvalidOperationException("pepperLetters configuration is missing.");
        ArgumentNullException.ThrowIfNull(pepperLetters);
        var pepperLength = configuration.GetValue<int>("pepperLength");
        if (pepperLength <= 0)
        {
            throw new ArgumentOutOfRangeException(null, "The pepper length number cannot be less than 0.");
        }
        return new SaltAndPepperEncryption(pepperLetters, pepperLength);
    });
    builder.Services.AddScoped<IUserManager, UserRepositoryManager>();
    builder.Services.AddSingleton<IJwtTokenManager, SelfJwtTokenManager>(provider =>
    {
        var (secretKey, issuer, audience) = extractJwtSettings(configuration);
        return new SelfJwtTokenManager(secretKey, issuer, audience);
    });

    builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();

}

WebApplication? buildApiLevel(WebApplicationBuilder builder, ConfigurationManager configuration)
{

    builder.Services.AddControllers();

    var app = builder.Build();

    app.MapDefaultEndpoints();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
       {
           var serviceName = configuration.GetValue<string>("service-name") ?? throw new InvalidOperationException("service-name configuration is missing.");
           ArgumentNullException.ThrowIfNullOrEmpty(serviceName);
           options.WithTitle(serviceName).
           WithTheme(ScalarTheme.Solarized).
           WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
       });
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    return app;
}

(string, string, string) extractJwtSettings(ConfigurationManager configuration)
{
    var secretKey = configuration.GetValue<string>("jwt:secretKey") ?? throw new InvalidOperationException("jwt:secretKey configuration is missing.");
    ArgumentNullException.ThrowIfNull(secretKey);
    var issuer = configuration.GetValue<string>("jwt:issuer") ?? throw new InvalidOperationException("jwt:issuer configuration is missing.");
    ArgumentNullException.ThrowIfNull(issuer);
    var audience = configuration.GetValue<string>("jwt:audience") ?? throw new InvalidOperationException("jwt:audience configuration is missing.");
    ArgumentNullException.ThrowIfNull(audience);
    return (secretKey, issuer, audience);
}