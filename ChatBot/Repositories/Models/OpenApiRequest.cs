using System.Text.Json.Serialization;

namespace ChatBot.Repositories.Models;


public class OpenApiRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("messages")]
    public List<OpenApiResponse.Message> Messages { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }
}
