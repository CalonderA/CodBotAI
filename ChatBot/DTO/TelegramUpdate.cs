using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace ChatBot.DTO;

public class TelegramUpdate
{
    public int UpdateId { get; set; }
    public TelegramMessage? Message { get; set; }
    public TelegramMessage? EditedMessage { get; set; }
}
