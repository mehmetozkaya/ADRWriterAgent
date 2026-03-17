var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ADRWriter_ApiService>("apiservice");

builder.AddProject<Projects.ADRWriter_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
