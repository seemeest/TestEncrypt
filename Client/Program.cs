using Client.ClientEncryptionService;
using System.Text;

var encryptionService = new ClientEncryptionService("E546C8DF278CD5931069B522E695D4F2");
string original = "Sensitive Data";
string encrypted = encryptionService.Encrypt(original);

using (var client = new HttpClient())
{
    var content = new StringContent($"\"{encrypted}\"", Encoding.UTF8, "application/json");
    var response = await client.PostAsync("https://yourserver/api/home/receive", content);
    var encryptedResponse = await response.Content.ReadAsStringAsync();

    Console.WriteLine($"Encrypted response: {encryptedResponse}");
}