using ChatBot.Commands;
using ChatBot.DTO;
using ChatBot.Repositories.Interfaces;
using ChatBot.Repositories.Models;
using Telegram.Bot;

namespace ChatBot;

public class TelegramUpdateProcessor
{
    private readonly IEnumerable<IBotCommand> _commands;
    private readonly ITelegramBotClient _botClient;
    private readonly IChatApiClient _chatClient;
    private readonly IChatModelRepository _chatModelRepository;
    private readonly ILogger<TelegramUpdateProcessor> _logger;


    public TelegramUpdateProcessor(
        IEnumerable<IBotCommand> commands,
        ITelegramBotClient botClient,
        IChatApiClient chatClient,
        IChatModelRepository chatModelRepository,
        ILogger<TelegramUpdateProcessor> logger)
    {
        _commands = commands;
        _botClient = botClient;
        _chatClient = chatClient;
        _chatModelRepository = chatModelRepository;
        _logger = logger;
    }

    public async Task HandleAsync(TelegramUpdate update)
    {
        if (update.Message == null || string.IsNullOrWhiteSpace(update.Message.Text))
            return;

        var chatId = update.Message.Chat.Id;
        var text = update.Message.Text.Trim();

        _logger.LogInformation("Получено сообщение от чата {ChatId}: {Text}", chatId, text);

        if (text.StartsWith("/"))
        {
            var cmd = text.Split(' ', 2)[0];
            var command = _commands.FirstOrDefault(c => c.Trigger.Equals(cmd, StringComparison.OrdinalIgnoreCase));
            if (command != null)
            {
                _logger.LogInformation("Выполняется команда {Command}", cmd);
                await command.ExecuteAsync(update, _botClient, chatId);
                return;
            }
            else
            {
                _logger.LogWarning("Неизвестная команда: {Command}", cmd);
                await _botClient.SendTextMessageAsync(chatId, "Неизвестная команда. Используйте /help");
                return;
            }
        }

        //добавили сохранение сообщения от пользователя в историю
        await _chatModelRepository.AddMessageAsync(chatId, new OpenApiResponse.Message { Role = "user", Content = text });
        var history = await _chatModelRepository.GetHistoryAsync(chatId);
        try
        {
            //добавили передачу истории в Chat API, чтобы он мог учитывать предыдущие сообщения при формировании ответа
            _logger.LogInformation("Отправка запроса в Chat API");
            var result = await _chatClient.SendMessageAsync(text, history);
            //добавили сохранение ответа от Chat API в историю
            await _chatModelRepository.AddMessageAsync(chatId, new OpenApiResponse.Message { Role = "assistant", Content = result });

            _logger.LogInformation("Ответ отправлен пользователю");
            await _botClient.SendTextMessageAsync(chatId, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при вызове Chat API");
            await _botClient.SendTextMessageAsync(chatId, "Ошибка при вызове Chat API: " + ex.Message);
        }

    }
}
