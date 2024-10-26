var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var ollama = builder.AddOllama(name: "ollama", port: null)
                    .AddModel("phi3.5")
                    .WithOpenWebUI()
                    .WithDataVolume()
                    .PublishAsContainer();

var apiService = builder.AddProject<Projects.AspireAIBlazorChatBot_ApiService>("apiservice");

builder.AddProject<Projects.AspireAIBlazorChatBot_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(ollama)
    .WithReference(apiService);

builder.Build().Run();
