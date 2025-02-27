var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Gateway_Api>("gateway")
                .WithEndpoint("http", endpoint => endpoint.IsProxied = false)
                 .WithEndpoint("https", endpoint => endpoint.IsProxied = false); ;
builder.Build().Run();
