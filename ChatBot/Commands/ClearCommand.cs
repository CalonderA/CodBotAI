using ChatBot.DTO;
using ChatBot.Repositories.Interfaces;
using Telegram.Bot;

namespace ChatBot.Commands;

public class ClearCommand : IBotCommand
{
    private readonly IChatModelRepository _chatModelRepository;
    private readonly ILogger<ClearCommand> _logger;

    public ClearCommand(IChatModelRepository chatModelRepository, ILogger<ClearCommand> logger)
    {
        _chatModelRepository = chatModelRepository;
        _logger = logger;
    }

    public string Trigger => "/clear";

    public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
    {
        _logger.LogInformation("Начало выполнения команды /clear для чата {ChatId}", chatId);

        await _chatModelRepository.ClearHistoryAsync(chatId);

        var message = "🗑️ История очищена\n\n" +
                      "Начните новый диалог -- я буду отвечать с чистого листа.";

        await bot.SendTextMessageAsync(chatId, message);

        _logger.LogInformation("Команда /clear успешно выполнена для чата {ChatId}. Результат: история очищена", chatId);
    }
}
