using Gateway.Application.Encryption;
using Gateway.Application.UserManager;
using Gateway.Infrastructure.db;
using Gateway.Infrastructure.UserRepository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddSingleton<IConfiguration>(configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddServiceDefaults();

buildInfrastructure(builder);
buildApplication(builder);
buildApiLevel(builder);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
   {
       string serviceName = builder.Configuration["service-name"] ?? string.Empty;
       options.WithTitle(serviceName).
       WithTheme(ScalarTheme.Solarized).
       WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
   });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();


void buildInfrastructure(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration["DbConnectionString"];
    ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString);
    builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new NpgsqlDbConnectionFactory(connectionString));
    builder.Services.AddScoped<IUserRepository, DbUserRepository>();


}

void buildApplication(WebApplicationBuilder builder)
{

    builder.Services.AddScoped<IPasswordEncryption, SaltAndPepperEncryption>(provider =>
    {
        var pepperLetters = configuration.GetValue<string>("pepperLetters");
        ArgumentNullException.ThrowIfNull(pepperLetters);
        var pepperLength = configuration.GetValue<int>("pepperLength");
        if (pepperLength < 0)
        {
            throw new ArgumentOutOfRangeException(null, "The pepper length number cannot be less than 0.");
        }
        return new SaltAndPepperEncryption(pepperLetters, pepperLength);
    });
    builder.Services.AddScoped<IUserManager, UserRepositoryManager>();

}

void buildApiLevel(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();

}
