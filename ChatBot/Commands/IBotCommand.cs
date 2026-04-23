using ChatBot.DTO;
using Telegram.Bot;

namespace ChatBot.Commands;

public interface IBotCommand
{
    string Trigger { get; }
    Task ExecuteAsync(TelegramUpdate update, ITelegramBotClient bot, long chatId);

}
