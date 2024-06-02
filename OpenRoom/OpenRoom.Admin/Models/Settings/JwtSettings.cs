using System.Text.Json.Serialization;

namespace OpenRoom.Admin.Models.Settings;

public class JwtSettings
{
    public const string SettingKey = "JwtSettings";//轉成JSON吐出物件
    
    [JsonPropertyName("Issuer")]
    public string Issuer { get; set; }
    [JsonPropertyName("SignKey")]
    public string SignKey { get; set; }
}