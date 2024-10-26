using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var ollama = builder.AddOllama(name: "ollama", port: null)
                    .AddModel("phi3.5")
                    .WithOpenWebUI()
                    .WithDataVolume();

var apiService = builder.AddProject<Projects.AspireBlazorAIChatBot_ApiService>("apiservice");

builder.AddProject<Projects.AspireBlazorAIChatBot_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(ollama);

builder.Build().Run();
