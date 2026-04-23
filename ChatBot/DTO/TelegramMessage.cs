using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace ChatBot.DTO;

public class TelegramMessage
{
    [Required]
    public int MessageId { get; set; }
    public Chat Chat { get; set; } = new Chat();
    [MaxLength(4096)]
    public string? Text { get; set; }
    public int Date { get; set; }
}
