var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireBlazorAIChatBot_ApiService>("apiservice");

builder.AddProject<Projects.AspireBlazorAIChatBot_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
