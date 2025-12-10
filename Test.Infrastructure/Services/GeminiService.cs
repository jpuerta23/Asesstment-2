using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Test.Application.Interfaces;
using Test.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Test.Infrastructure.Services;

public class GeminiService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GeminiService> _logger;

    public GeminiService(HttpClient httpClient, IConfiguration configuration, ILogger<GeminiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GetProfileSuggestionAsync(string cargo, string nivelEducativo)
    {
        var apiKey = _configuration["Gemini:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogWarning("Gemini API Key is missing.");
            return "API Key missing. Cannot generate suggestion.";
        }

        var prompt = $"Genera un perfil profesional corto (max 50 palabras) para un {cargo} con nivel educativo {nivelEducativo}.";
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        int maxRetries = 3;
        int delay = 1000;

        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                var response = await _httpClient.PostAsync(url, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseString);
                    return geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "No suggestion generated.";
                }

                if ((int)response.StatusCode == 429) // Too Many Requests
                {
                    _logger.LogWarning($"Rate limit exceeded. Retrying in {delay}ms...");
                    await Task.Delay(delay);
                    delay *= 2; // Exponential backoff
                    continue;
                }

                _logger.LogError($"Gemini API Error: {response.StatusCode}");
                return "Error generating suggestion.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Gemini API");
                return "Error calling AI service.";
            }
        }

        return "Service unavailable after retries.";
    }

    public async Task<string> AnswerQuestionAsync(string question, IEnumerable<Empleado> empleados)
    {
        var apiKey = _configuration["Gemini:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogWarning("Gemini API Key is missing.");
            return "API Key missing. Cannot answer question.";
        }

        var empleadosData = JsonSerializer.Serialize(empleados.Select(e => new
        {
            e.Nombres,
            e.Apellidos,
            Cargo = e.Cargo?.Nombre,
            Departamento = e.Departamento?.Nombre,
            e.Salario,
            e.FechaIngreso
        }));

        var prompt = $"Contexto: Tienes la siguiente lista de empleados: {empleadosData}. Pregunta: {question}. Responde basándote solo en los datos proporcionados.";
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        int maxRetries = 3;
        int delay = 1000;

        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                var response = await _httpClient.PostAsync(url, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseString);
                    return geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "No answer generated.";
                }

                if ((int)response.StatusCode == 429) // Too Many Requests
                {
                    _logger.LogWarning($"Rate limit exceeded. Retrying in {delay}ms...");
                    await Task.Delay(delay);
                    delay *= 2; // Exponential backoff
                    continue;
                }

                _logger.LogError($"Gemini API Error: {response.StatusCode}");
                return "Error generating answer.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Gemini API");
                return "Error calling AI service.";
            }
        }

        return "Service unavailable after retries.";
    }
}

// Helper classes for deserialization
public class GeminiResponse
{
    [JsonPropertyName("candidates")]
    public List<Candidate> Candidates { get; set; }
}

public class Candidate
{
    [JsonPropertyName("content")]
    public Content Content { get; set; }
}

public class Content
{
    [JsonPropertyName("parts")]
    public List<Part> Parts { get; set; }
}

public class Part
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}
