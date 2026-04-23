using ChatBot.DTO;
using ChatBot.Repositories.Interfaces;
using Telegram.Bot;

namespace ChatBot.Commands;

public class StatsCommand : IBotCommand
{
    private readonly IChatModelRepository _chatModelRepository;
    private readonly ILogger<StatsCommand> _logger;

    public StatsCommand(IChatModelRepository chatModelRepository, ILogger<StatsCommand> logger)
    {
        _chatModelRepository = chatModelRepository;
        _logger = logger;
    }

    public string Trigger => "/stats";

    public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
    {
        _logger.LogInformation("Начало выполнения команды /stats для чата {ChatId}", chatId);

        var stats = await _chatModelRepository.GetStatsAsync(chatId);

        var message = $"📊 Статистика чата\n\n" +
                      $"Сообщений от вас: {stats.UserMessages}\n" +
                      $"Ответов бота: {stats.AssistantMessages}\n" +
                      $"Всего сообщений: {stats.TotalMessages}\n\n" +
                      $"Токенов (приблизительно):\n" +
                      $"  - От вас: ~{stats.EstimatedUserTokens}\n" +
                      $"  - От бота: ~{stats.EstimatedAssistantTokens}\n" +
                      $"  - Всего: ~{stats.EstimatedTotalTokens}";

        await bot.SendTextMessageAsync(chatId, message);

        _logger.LogInformation("Команда /stats успешно выполнена для чата {ChatId}. Результат: {TotalMessages} сообщений, {TotalTokens} токенов",
            chatId, stats.TotalMessages, stats.EstimatedTotalTokens);
    }
}
