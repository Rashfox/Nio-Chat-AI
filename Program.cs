using Photino.NET;
using System.Diagnostics;
using blazor.Components;
using blazor.Services;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("shell32.dll", SetLastError = true)]
    static extern int SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

    const int SW_HIDE = 0;
    const uint WM_SETICON = 0x0080;
    const int ICON_SMALL = 0;
    const int ICON_BIG = 1;
    const uint IMAGE_ICON = 1;
    const uint LR_LOADFROMFILE = 0x0010;

    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            SetCurrentProcessExplicitAppUserModelID("Nio.Chat.App");
        }
        catch { }

        var handle = GetConsoleWindow();
        ShowWindow(handle, SW_HIDE);
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

        var builder = WebApplication.CreateBuilder(args);

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

        // Bind to a random port on 127.0.0.1 to avoid port collisions and IPv6 localhost issues
        app.Urls.Add("http://127.0.0.1:0");
        app.Start();
        
        string appUrl = app.Urls.FirstOrDefault() ?? "http://127.0.0.1:5000";

        // Create the native Desktop Window
        try
        {
            var icoPath = Path.Combine(AppContext.BaseDirectory, "app.ico");
            if (!File.Exists(icoPath))
            {
                icoPath = Path.Combine(Directory.GetCurrentDirectory(), "app.ico");
            }

            Task.Run(async () =>
            {
                for (int i = 0; i < 30; i++)
                {
                    await Task.Delay(200);
                    var hWnd = FindWindow(null, "Nio AI Chat");
                    if (hWnd != IntPtr.Zero)
                    {
                        if (File.Exists(icoPath))
                        {
                            var hSmall = LoadImage(IntPtr.Zero, icoPath, IMAGE_ICON, 16, 16, LR_LOADFROMFILE);
                            var hBig = LoadImage(IntPtr.Zero, icoPath, IMAGE_ICON, 32, 32, LR_LOADFROMFILE);
                            if (hSmall != IntPtr.Zero) SendMessage(hWnd, WM_SETICON, (IntPtr)ICON_SMALL, hSmall);
                            if (hBig != IntPtr.Zero) SendMessage(hWnd, WM_SETICON, (IntPtr)ICON_BIG, hBig);
                        }
                        break;
                    }
                }
            });

            var window = new PhotinoWindow()
                .SetTitle("Nio AI Chat")
                .SetIconFile(icoPath)
                .SetUseOsDefaultSize(false)
                .SetSize(1400, 900)
                .Center()
                .Load(appUrl);
            window.WaitForClose();
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllText("crash.txt", ex.ToString());
        }
    }
}
