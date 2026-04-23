using ChatBot.Repositories.Models;

namespace ChatBot.Repositories.Interfaces;


public interface IChatApiClient
{
    Task<string> SendMessageAsync(string userMessage, IEnumerable<OpenApiResponse.Message> history);
}