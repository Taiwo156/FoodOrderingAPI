using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using APItask.Service;

public class ChatbotService : IChatbotService
{
    private readonly HttpClient _httpClient;
    private readonly string _mistralApiKey;
    private readonly string _mistralApiUrl = "https://api.mistral.ai/v1/chat/completions";

    public ChatbotService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory?.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _mistralApiKey = configuration?["MistralAI:ApiKey"] ?? throw new ArgumentNullException(nameof(configuration));

        if (string.IsNullOrEmpty(_mistralApiKey))
        {
            throw new InvalidOperationException("Mistral API key is not configured");
        }
    }

    public async Task<object> GetChatbotResponse(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return "Please provide a valid message.";
        }

        // List of common greetings
        var greetings = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "hi", "hello", "hey", "greetings", "good morning", "good afternoon", "good evening"
            };

        // Provide greeting and menu options if the message is a greeting
        if (greetings.Contains(message.Trim()))
        {
            return "Hello! I'm here to help you with your food ordering needs. What can I assist you with today? Please choose an option by typing the corresponding number:\n" +
                   "1️⃣ View menu 🍽️\n" +
                   "2️⃣ Order food 🛒\n" +
                   "3️⃣ Track my order 📦\n" +
                   "4️⃣ Contact support ☎️\n" +
                   "5️⃣ Exit ❌";
        }

        // Handle user input options
        switch (message.Trim())
        {
            case "1":
                return "Here is the menu:\n🍕 Pizza - $10\n🍔 Burger - $8\n🍣 Sushi - $12\n🥗 Salad - $6\n\nType the number of the item you want to order.";
            case "2":
                return "What would you like to order? Please type 'menu' to see options.";
            case "3":
                return "Enter your order ID to track your order.";
            case "4":
                return "You can contact support at +123456789 or email support@foodapp.com.";
            case "5":
                return "Goodbye! Hope to see you again. 😊";
        }

        // Call Mistral AI for other queries
        var requestBody = new
        {
            model = "open-mistral-7b",
            messages = new[]
            {
                    new { role = "system", content = "You are a helpful food ordering assistant." },
                    new { role = "user", content = message }
                }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _mistralApiKey);

        try
        {
            var response = await _httpClient.PostAsync(_mistralApiUrl, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var mistralResponse = JsonSerializer.Deserialize<MistralResponse>(responseBody);

            return mistralResponse?.Choices?.FirstOrDefault()?.Message?.Content
                ?? "Sorry, I couldn't understand that. Please try again.";
        }
        catch (HttpRequestException ex)
        {
            return $"⚠️ Error communicating with AI: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"⚠️ An unexpected error occurred: {ex.Message}";
        }
    }
}

// Define Mistral API Response Model
public class MistralResponse
{
    [JsonPropertyName("choices")]
    public List<MistralChoice> Choices { get; set; } = new List<MistralChoice>();
}

public class MistralChoice
{
    [JsonPropertyName("message")]
    public MistralMessage Message { get; set; } = new MistralMessage();
}

public class MistralMessage
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}
