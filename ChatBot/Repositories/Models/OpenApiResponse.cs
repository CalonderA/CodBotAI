using System.Text.Json.Serialization;

namespace ChatBot.Repositories.Models;

public class OpenApiResponse
{
    [JsonPropertyName("choices")]
    public Choice[]? Choices { get; set; }

    public class Choice
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; } = new Message();
    }

    public class Message
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }

}
