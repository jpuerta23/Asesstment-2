using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Test.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Test.Domain.Entities;

namespace Test.Infrastructure.Services;

public class ChatGPTService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ChatGPTService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> AnswerQuestionAsync(string question, IEnumerable<Empleado> contextData)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];
        var model = _configuration["OpenAI:Model"];

        if (string.IsNullOrWhiteSpace(apiKey))
            return "OpenAI API Key not configured.";

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        // Convertir empleados en texto
        string contextSummary = string.Join("\n", contextData.Select(e =>
            $"ID: {e.Id}, Nombre: {e.Nombres} {e.Apellidos}, Cargo: {e.Cargo?.Nombre}, " +
            $"Departamento: {e.Departamento?.Nombre}, Estado: {e.EstadoEmpleado?.Nombre}, " +
            $"Salario: {e.Salario}"
        ));

        string prompt = $@"
Eres una IA que solo puede responder usando la siguiente base de datos:

{contextSummary}

Pregunta del usuario: {question}

Si la respuesta no puede determinarse con estos datos, responde: 
'No existen datos para responder esa pregunta.'
";

        var requestBody = new
        {
            model = model,
            messages = new[]
            {
                new { role = "system", content = "Eres un asistente experto en análisis de datos internos." },
                new { role = "user", content = prompt }
            },
            max_tokens = 300
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return $"OpenAI error: {response.StatusCode}\n{raw}";
        }

        try
        {
            using var doc = JsonDocument.Parse(raw);
            string answer = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return answer ?? "No answer generated.";
        }
        catch
        {
            return "Error parsing OpenAI response.";
        }
    }
}
