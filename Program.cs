using Photino.NET;
using System.Diagnostics;
using blazor.Components;
using blazor.Services;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
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

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = AppContext.BaseDirectory
        });

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

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
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseAntiforgery();
        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Bind to a random port on 127.0.0.1 to avoid port collisions and IPv6 localhost issues
        app.Urls.Add("http://127.0.0.1:0");
        app.Start();
        
        string appUrl = app.Urls.FirstOrDefault() ?? "http://127.0.0.1:5000";

        // Create the native Desktop Window
        var window = new PhotinoWindow()
            .SetTitle("Nio AI Chat")
            .SetIconFile("app.ico")
            .SetUseOsDefaultSize(false)
            .SetSize(1400, 900)
            .Center()
            .Load(appUrl);

        // Wait for the user to close the window
        window.WaitForClose();
    }
}
