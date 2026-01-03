using System.Text.Json;
using Guidex_Backend.Infrastructure.Interface;
using Guidex_Backend.Infrastructure.Prompts;
using Guidex_Backend.Infrastructure.Responses;

namespace Guidex_Backend.Infrastructure
{
    public class OllamaClient : IOllamaClient
    {
        private readonly HttpClient _http;
        

        public OllamaClient(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:11434");
        }

        public async Task<LlmResponse> GenerateAsync(string model, string messages, List<Message>? history = null, List<string>? contexts = null)
        {
            var prompt = new PromptTemplate(SystemPrompts.contextOriantedAssistance);
            if (history != null)
            {
                prompt.AddMessage(history);
            }
            
            prompt.AddContext(contexts ?? new List<string>());

            Console.WriteLine($"contexts?.Count ?? 0: {contexts?.Count ?? 0}");

            if (contexts != null && contexts.Count > 0)
            {
                Console.WriteLine("Contexts added to the prompt:");
                prompt.AddContext(contexts);
            }
            
            var renderedPrompt = prompt.Render(messages);
            Console.WriteLine(renderedPrompt);
            var request = new
            {
                model = model,
                prompt = renderedPrompt,
                stream = false
            };

            var response = await _http.PostAsJsonAsync("/api/generate", request);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            var json = await JsonDocument.ParseAsync(stream);
            var cleanedResponse = Util.LlmOutputCleaner.RemoveThinkBlocks(json.RootElement.GetProperty("response").GetString()!);
            Console.WriteLine("Current Prompt ===================================================:");
            PromptHistory.History.ForEach(msg=> Console.WriteLine($"Role: {msg.Role}, Content: {msg.Content}")); 
            Console.WriteLine("Current Prompt ===================================================:");
            return new LlmResponse
            {
                UserMessage = new Message("user", messages),
                LlmMessage = new Message("assistant", cleanedResponse)
            };
           
        }
    }
}