var builder = DistributedApplication.CreateBuilder(args);

// Optional: leave Grafana/Prometheus commented as-is

var username = builder.AddParameter("pg-username", "admin");
var password = builder.AddParameter("pg-password", "admin");

// Rename to AMIS11 for consistency
var postgres = builder.AddPostgres("db", username, password, port: 5432)
    .WithDataVolume();

var database = postgres.AddDatabase("AMIS11");

var api = builder.AddProject<Projects.Server>("webapi")
    // Inject connection string + dependency
    .WithReference(database)
    .WaitFor(database);

var blazor = builder.AddProject<Projects.Client>("blazor")
    // Enable service discovery to the API
    .WithReference(api);

using var app = builder.Build();

await app.RunAsync();
