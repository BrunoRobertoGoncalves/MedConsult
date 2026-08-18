using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("postgres")
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("mydb");

var cache = builder.AddRedis("cache")
    .WithLifetime(ContainerLifetime.Persistent);

var api = builder.AddProject<API>("api")
    .WithReference(db)
    .WithReference(cache)
    .WaitFor(db);

builder.Build().Run();