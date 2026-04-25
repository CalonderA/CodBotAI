using ChatBot.DTO;
using ChatBot.Repositories.Interfaces;
using ChatBot.Repositories.Models;
using Telegram.Bot;

namespace ChatBot.Commands;

public class JokeCommand : IBotCommand
{
    private readonly ILogger<JokeCommand> _logger;
    private readonly IChatApiClient _chatClient;

    public JokeCommand(ILogger<JokeCommand> logger, IChatApiClient chatClient)
    {
        _logger = logger;
        _chatClient = chatClient;
    }

    public string Trigger => "/joke";

    public async Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId)
    {
        _logger.LogInformation("Команда /joke выполнена для чата {ChatId}", chatId);

        try
        {
            var jokePrompt = "Расскажи короткую смешную шутку или забавную цитату на русском языке. Будь креативным и оригинальным!";
            var emptyHistory = new List<OpenApiResponse.Message>();

            var joke = await _chatClient.SendMessageAsync(jokePrompt, emptyHistory);

            await bot.SendTextMessageAsync(chatId, $"😄 {joke}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при генерации шутки");
            await bot.SendTextMessageAsync(chatId, "Не удалось сгенерировать шутку 😔");
        }
    }
}
