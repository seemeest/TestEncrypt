using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebApplication3.DataBase;

namespace WebApplication3
{
    // Services/ChatEncryptionService.cs
    public class ChatEncryptionService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly ApplicationContext _context;
        private readonly ILogger<ChatEncryptionService> _logger;

        public ChatEncryptionService(IDataProtectionProvider dataProtectionProvider, ApplicationContext context, ILogger<ChatEncryptionService> logger)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _context = context;
            _logger = logger;
        }

        public async Task<(string publicKey, string privateKey)> GenerateAndStoreKeysAsync(string chatId)
        {
            var protector = _dataProtectionProvider.CreateProtector(chatId);
            var publicKey = Convert.ToBase64String(protector.Protect(Guid.NewGuid().ToByteArray()));
            var privateKey = Convert.ToBase64String(protector.Protect(Guid.NewGuid().ToByteArray()));

            var chatKey = new ChatKey
            {
                ChatId = chatId,
                PublicKey = publicKey,
                PrivateKey = privateKey
            };

            _context.ChatKeys.Add(chatKey);
            await _context.SaveChangesAsync();

            return (publicKey, privateKey);
        }

        public async Task<string> EncryptMessageAsync(string chatId, string message)
        {
            var chatKey = await _context.ChatKeys.FirstOrDefaultAsync(k => k.ChatId == chatId);
            if (chatKey == null)
            {
                _logger.LogError($"Chat key for chatId {chatId} not found.");
                throw new Exception("Chat key not found");
            }

            var protector = _dataProtectionProvider.CreateProtector(chatKey.PublicKey);
            return protector.Protect(message);
        }

        public async Task<string> DecryptMessageAsync(string chatId, string encryptedMessage)
        {
            var chatKey = await _context.ChatKeys.FirstOrDefaultAsync(k => k.ChatId == chatId);
            if (chatKey == null)
            {
                _logger.LogError($"Chat key for chatId {chatId} not found.");
                throw new Exception("Chat key not found");
            }

            var protector = _dataProtectionProvider.CreateProtector(chatKey.PrivateKey);
            try
            {
                return protector.Unprotect(encryptedMessage);
            }
            catch (CryptographicException ex)
            {
                _logger.LogError($"Decryption failed for chatId {chatId}. Exception: {ex.Message}");
                throw;
            }
        }
    }



}
