using System.Net.Http.Json;
using System.Text.Json;

namespace blazor.Services;

public sealed class OllamaChatService(HttpClient httpClient, IConfiguration configuration, IHostEnvironment environment)
{
    public async Task<string> SendAsync(IReadOnlyList<OllamaChatMessage> messages, CancellationToken cancellationToken = default)
    {
        var model = configuration["Ollama:Model"] ?? "llama3.2";
        var request = new
        {
            model,
            messages,
            stream = false
        };

        using var response = await httpClient.PostAsJsonAsync("api/chat", request, cancellationToken);
        var payload = await response.Content.ReadFromJsonAsync<OllamaResponse>(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new OllamaException(payload?.Error ?? "Ollama mengembalikan error.");
        }

        var assistantMessage = payload?.Message?.Content ?? string.Empty;
        var trainingMessages = messages
            .Append(new OllamaChatMessage("assistant", assistantMessage))
            .ToArray();
        var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var trainingDirectory = Path.Combine(appDataFolder, "NioChat", "App_Data");
        Directory.CreateDirectory(trainingDirectory);
        var trainingRecord = new
        {
            timestamp = DateTimeOffset.UtcNow,
            model,
            messages = trainingMessages
        };
        var trainingLine = JsonSerializer.Serialize(trainingRecord) + Environment.NewLine;
        await File.AppendAllTextAsync(
            Path.Combine(trainingDirectory, "training-conversations.jsonl"),
            trainingLine,
            cancellationToken);

        return assistantMessage;
    }
}

public sealed record OllamaChatMessage(string Role, string Content);
public sealed record OllamaResponse(OllamaMessage? Message, string? Error);
public sealed record OllamaMessage(string Content);
public sealed class OllamaException(string message) : Exception(message);
