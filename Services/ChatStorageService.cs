using System.Text.Json;
using System.Text.Json.Serialization;

namespace blazor.Services;

public class ChatStorageService
{
    private static readonly string StorageDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "NioChat");

    private static readonly string HistoryFile = Path.Combine(StorageDir, "history.json");
    private static readonly string ProfileFile = Path.Combine(StorageDir, "profile.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public string? GetUserName()
    {
        try
        {
            if (!File.Exists(ProfileFile))
                return null;

            var json = File.ReadAllText(ProfileFile);
            var profile = JsonSerializer.Deserialize<UserProfileData>(json, JsonOptions);
            return string.IsNullOrWhiteSpace(profile?.UserName) ? null : profile.UserName.Trim();
        }
        catch
        {
            return null;
        }
    }

    public void SaveUserName(string userName)
    {
        try
        {
            Directory.CreateDirectory(StorageDir);
            var profile = new UserProfileData { UserName = userName.Trim() };
            var json = JsonSerializer.Serialize(profile, JsonOptions);
            File.WriteAllText(ProfileFile, json);
        }
        catch { }
    }

    public void ClearHistory()
    {
        try
        {
            if (File.Exists(HistoryFile))
            {
                File.Delete(HistoryFile);
            }
        }
        catch { }
    }

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

public class UserProfileData
{
    public string UserName { get; set; } = string.Empty;
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
