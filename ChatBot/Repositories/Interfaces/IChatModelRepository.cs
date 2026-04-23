using ChatBot.DTO;
using ChatBot.Repositories.Models;

namespace ChatBot.Repositories.Interfaces;

public interface IChatModelRepository
{
    Task<List<OpenApiResponse.Message>> GetHistoryAsync(long chatId);
    Task AddMessageAsync(long chatId, OpenApiResponse.Message message);
    Task<ChatStats> GetStatsAsync(long chatId);
    Task ClearHistoryAsync(long chatId);
    Task<bool> RemoveLastMessageAsync(long chatId);
}