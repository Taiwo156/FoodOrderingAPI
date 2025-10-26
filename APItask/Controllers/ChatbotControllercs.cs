using APItask.Core;
using APItask.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/chatbot")]
public class ChatbotController : ControllerBase
{
    private readonly IChatbotService _chatbotService;

    public ChatbotController(IChatbotService chatbotService)
    {
        _chatbotService = chatbotService;
    }

    [HttpPost]
    public async Task<IActionResult> Chatbot([FromBody] ChatRequest request)
    {
        var result = await _chatbotService.GetChatbotResponse(request.Message);
        return Ok(result);
    }
}

public class ChatRequest
{
    public string? Message { get; set; }
}
