namespace ChatBot.DTO;

public class ChatStats
{
    public int UserMessages { get; set; }
    public int AssistantMessages { get; set; }
    public int TotalMessages => UserMessages + AssistantMessages;
    public int EstimatedUserTokens { get; set; }
    public int EstimatedAssistantTokens { get; set; }
    public int EstimatedTotalTokens => EstimatedUserTokens + EstimatedAssistantTokens;
}
