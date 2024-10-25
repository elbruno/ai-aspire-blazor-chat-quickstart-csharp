var builder = DistributedApplication.CreateBuilder(args);

// add a new value to the configuration for the default LLM model with the value "gpt-3.5-turbo"
var modelName = "phi3.5";

var ollama = builder.AddOllama("ollama")
                    .AddModel(modelName)
                    .WithDataVolume();

var apiService = builder.AddProject<Projects.AspireBlazorAIChatBot_ApiService>("apiservice");

builder.AddProject<Projects.AspireBlazorAIChatBot_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(ollama);

builder.Build().Run();
