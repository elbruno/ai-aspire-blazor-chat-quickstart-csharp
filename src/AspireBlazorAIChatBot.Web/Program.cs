using AspireBlazorAIChatBot.Web;
using AspireBlazorAIChatBot.Web.Components;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using System.ClientModel;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add logging with detailed information
builder.Services.AddLogging();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();
builder.Logging.AddFilter("Microsoft", LogLevel.Information);
builder.Logging.AddFilter("System", LogLevel.Information);
builder.Logging.AddFilter("AspireBlazorAIChatBot", LogLevel.Debug);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apiservice");
    });

builder.Services.AddSingleton<ILogger>(static serviceProvider =>
{
    var lf = serviceProvider.GetRequiredService<ILoggerFactory>();
    return lf.CreateLogger(typeof(Program));
});

// register chat client
builder.Services.AddSingleton<IChatClient>(static serviceProvider =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();

    var logger = serviceProvider.GetRequiredService<ILogger>();
    logger.LogInformation("AZURE_OPENAI_ENDPOINT: {0}", config["AZURE_OPENAI_ENDPOINT"]);
    logger.LogInformation("AZURE_OPENAI_DEPLOYMENT: {0}", config["AZURE_OPENAI_DEPLOYMENT"]);

    return new AzureOpenAIClient(
        new Uri(config["AZURE_OPENAI_ENDPOINT"]!),
        new Azure.Identity.DefaultAzureCredential())
            .AsChatClient(modelId: config["AZURE_OPENAI_DEPLOYMENT"]!);

    // var endpoint = config["AZURE_OPENAI_ENDPOINT"];
    // var modelId = config["AZURE_OPENAI_MODEL"];
    // var apiKey = config["AZURE_OPENAI_APIKEY"];
    // var credential = new ApiKeyCredential(apiKey);
    // return new AzureOpenAIClient(new Uri(endpoint), credential)
    //     .AsChatClient(modelId: modelId);

});

// register chat nessages
builder.Services.AddSingleton<List<ChatMessage>>(static serviceProvider =>
{
    return new List<ChatMessage>()
    { new ChatMessage(ChatRole.System, "You are a useful assistant that replies using short and precise sentences.")};
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
