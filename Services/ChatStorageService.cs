using System.Text.Json;
using System.Text.Json.Serialization;

namespace blazor.Services;

public class ChatStorageService
{
    private static readonly string StorageDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "NioChat");

    private static readonly string HistoryFile = Path.Combine(StorageDir, "history.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public List<ChatSessionData> LoadSessions()
    {
        try
        {
            if (!File.Exists(HistoryFile))
                return [];

            var json = File.ReadAllText(HistoryFile);
            var sessions = JsonSerializer.Deserialize<List<ChatSessionData>>(json, JsonOptions);
            return sessions ?? [];
        }
        catch
        {
            return [];
        }
    }

    public void SaveSessions(List<ChatSessionData> sessions)
    {
        try
        {
            Directory.CreateDirectory(StorageDir);
            var json = JsonSerializer.Serialize(sessions, JsonOptions);
            File.WriteAllText(HistoryFile, json);
        }
        catch { }
    }
}

public class ChatSessionData
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset UpdatedAt { get; set; }
    public List<ChatMessageData> Messages { get; set; } = [];
}

public class ChatMessageData
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
