using Application.UserManager;
using Infrastructure.db;
using Infrastructure.UserRepository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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

}

void buildApplication(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IUserRepository, DbUserRepository>();
}

void buildApiLevel(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IUserManager, SaltAndPepperUserManager>();
    builder.Services.AddControllers();

}
