using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;

namespace ChatbotApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatClient _chatClient;

        public ChatController(IConfiguration config)
        {
            var apiKey = config["OpenAI:ApiKey"];
            _chatClient = new ChatClient(model: "gpt-4o", apiKey);
        }

        [HttpPost("send")]
        public async Task<IActionResult> GetChatResponse([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message cannot be empty.");

            try
            {
                ChatCompletion completion = await _chatClient.CompleteChatAsync(request.Message);
                return Ok(new { response = completion.Content[0].Text });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }
}
