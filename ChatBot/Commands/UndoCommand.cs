using ChatBot.DTO;
using ChatBot.Repositories.Interfaces;
using Telegram.Bot;

namespace ChatBot.Commands;

public class UndoCommand : IBotCommand
{
    private readonly ILogger<UndoCommand> _logger;
    private readonly IChatModelRepository _chatModelRepository;

    public UndoCommand(
        ILogger<UndoCommand> logger,
        IChatModelRepository chatModelRepository)
    {
        _logger = logger;
        _chatModelRepository = chatModelRepository;
    }

    public string Trigger => "/undo";

    public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
    {
        _logger.LogInformation("Начало выполнения команды /undo для чата {ChatId}", chatId);

        // Сначала удаляем ответ ассистента (последнее сообщение)
        var removedAssistant = await _chatModelRepository.RemoveLastMessageAsync(chatId);

        if (removedAssistant)
        {
            // Потом удаляем сообщение пользователя
            await _chatModelRepository.RemoveLastMessageAsync(chatId);
            await bot.SendTextMessageAsync(chatId, "↩️ Последний обмен сообщениями удалён.");
            _logger.LogInformation("Команда /undo успешно выполнена для чата {ChatId}. Результат: последняя пара сообщений удалена", chatId);
        }
        else
        {
            await bot.SendTextMessageAsync(chatId, "❌ История пуста. Нечего удалять.");
            _logger.LogInformation("Команда /undo выполнена для чата {ChatId}. Результат: история пуста", chatId);
        }
    }
}
