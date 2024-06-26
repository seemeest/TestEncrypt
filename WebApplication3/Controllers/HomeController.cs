using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ChatEncryptionService _chatEncryptionService;

        public HomeController(ChatEncryptionService chatEncryptionService)
        {
            _chatEncryptionService = chatEncryptionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChat(string chatId)
        {
            var keys = await _chatEncryptionService.GenerateAndStoreKeysAsync(chatId);
            return Ok(keys);
        }

        [HttpPost("encrypt")]
        public async Task<IActionResult> EncryptMessage(string chatId, string message)
        {
            var encryptedMessage = await _chatEncryptionService.EncryptMessageAsync(chatId, message);
            return Ok(encryptedMessage);
        }

        [HttpPost("decrypt")]
        public async Task<IActionResult> DecryptMessage(string chatId, string encryptedMessage)
        {
            var decryptedMessage = await _chatEncryptionService.DecryptMessageAsync(chatId, encryptedMessage);
            return Ok(decryptedMessage);
        }
    }
}
