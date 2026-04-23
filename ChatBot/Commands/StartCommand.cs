using ChatBot.DTO;
using Telegram.Bot;

namespace ChatBot.Commands;

public class StartCommand : IBotCommand
{
    private readonly ILogger<StartCommand> _logger;

    public StartCommand(ILogger<StartCommand> logger)
    {
        _logger = logger;
    }

    public string Trigger => "/start";
    public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
    {
        _logger.LogInformation("Команда /start выполнена для чата {ChatId}", chatId);
        await bot.SendTextMessageAsync(chatId, "Привет! Я OpenAI-бот. Отправь сообщение -- я передам его сторонней модели и верну ответ.\n/help для списка команд.");
    }

}