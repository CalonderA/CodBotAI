using ChatBot.DTO;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace ChatBot;


[ApiController]
[Route("api/[controller]")]
public class UpdateController : ControllerBase
{
    private readonly TelegramUpdateProcessor _processor;
    public UpdateController(TelegramUpdateProcessor processor)
    {
        _processor = processor;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] TelegramUpdate update)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        _ = Task.Run(() => _processor.HandleAsync(update));

        return Ok();            
    }
}
