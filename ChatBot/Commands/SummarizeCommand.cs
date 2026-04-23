using ChatBot.DTO;
using ChatBot.Repositories.Interfaces;
using ChatBot.Repositories.Models;
using Telegram.Bot;

namespace ChatBot.Commands;

public class SummarizeCommand : IBotCommand
{
    private readonly ILogger<SummarizeCommand> _logger;
    private readonly IChatModelRepository _chatModelRepository;
    private readonly IChatApiClient _chatApiClient;

    public SummarizeCommand(
        ILogger<SummarizeCommand> logger,
        IChatModelRepository chatModelRepository,
        IChatApiClient chatApiClient)
    {
        _logger = logger;
        _chatModelRepository = chatModelRepository;
        _chatApiClient = chatApiClient;
    }

    public string Trigger => "/summarize";

    public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
    {
        _logger.LogInformation("Начало выполнения команды /summarize для чата {ChatId}", chatId);

        var history = await _chatModelRepository.GetHistoryAsync(chatId);

        if (history.Count == 0)
        {
            await bot.SendTextMessageAsync(chatId, "📭 История пуста. Напишите что-нибудь, и я сделаю краткий пересказ.");
            _logger.LogInformation("Команда /summarize выполнена для чата {ChatId}. Результат: история пуста", chatId);
            return;
        }

        var summarizePrompt = "Кратко перескажи основную суть нашего диалога. Выдели ключевые темы и выводы.";

        var summarizeHistory = history.ToList();
        summarizeHistory.Add(new OpenApiResponse.Message
        {
            Role = "user",
            Content = summarizePrompt
        });

        try
        {
            var summary = await _chatApiClient.SendMessageAsync(summarizePrompt, summarizeHistory);

            var response = $"📝 Краткий пересказ диалога:\n\n{summary}";
            await bot.SendTextMessageAsync(chatId, response);

            _logger.LogInformation("Команда /summarize успешно выполнена для чата {ChatId}. Результат: саммари создано", chatId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании саммари для чата {ChatId}", chatId);
            await bot.SendTextMessageAsync(chatId, "❌ Ошибка при создании саммари. Попробуйте позже.");
        }
    }
}
