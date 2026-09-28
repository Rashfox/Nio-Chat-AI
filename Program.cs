using System.Diagnostics;
using blazor.Components;
using blazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Auto-start Ollama if it's not running
var ollamaProcesses = Process.GetProcessesByName("ollama");
if (ollamaProcesses.Length == 0)
{
    var startInfo = new ProcessStartInfo
    {
        FileName = "ollama",
        Arguments = "serve",
        UseShellExecute = false,
        CreateNoWindow = true
    };
    try
    {
        Process.Start(startInfo);
        Console.WriteLine("Ollama started successfully in the background.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to start Ollama automatically: {ex.Message}");
    }
}

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<ChatStorageService>();
builder.Services.AddHttpClient<OllamaChatService>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(configuration["Ollama:BaseUrl"] ?? "http://localhost:11434");
    client.Timeout = TimeSpan.FromMinutes(5);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();
app.UseStaticFiles();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
