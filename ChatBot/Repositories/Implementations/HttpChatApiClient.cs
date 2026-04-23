using System.Net.Http.Headers;
using System.Net.Http.Json;
using ChatBot.Repositories.Interfaces;
using ChatBot.Repositories.Models;
using ChatBot.Settings;
using Microsoft.Extensions.Options;

namespace ChatBot.Repositories.Implementations
{
    public class HttpChatApiClient : IChatApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ChatApiSettings _chatSettings;
        private readonly ILogger<HttpChatApiClient> _logger;

        public HttpChatApiClient(HttpClient httpClient, IOptions<ChatApiSettings> chatOptions, ILogger<HttpChatApiClient> logger)
        {
            _chatSettings = chatOptions.Value;
            _httpClient = httpClient;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_chatSettings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _chatSettings.ApiKey);
        }

        public async Task<string> SendMessageAsync(string userMessage, IEnumerable<OpenApiResponse.Message> history)
        {
            _logger.LogInformation("Отправка запроса к модели {Model}", _chatSettings.DefaultModel);

            var payload = new OpenApiRequest()
            {
                Model = _chatSettings.DefaultModel,
                Messages = history.ToList(),
                MaxTokens = 1000
            };

            var response = await _httpClient.PostAsJsonAsync("", payload);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadFromJsonAsync<OpenApiResponse?>();
            if (body?.Choices != null && body.Choices.Length > 0)
            {
                var content = body.Choices[0].Message.Content;
                _logger.LogInformation("Получен ответ от API: {Content}", content);
                return content;
            }

            _logger.LogWarning("API вернул пустой ответ");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
