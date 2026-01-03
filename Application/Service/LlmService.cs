using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guidex_Backend.Application.Interface;
using Guidex_Backend.Infrastructure.Interface;
using Guidex_Backend.Infrastructure.Prompts;
using Guidex_Backend.Util;
using Microsoft.OpenApi.Validations;

namespace Guidex_Backend.Application.Service
{
    public class LlmService : ILlmService
    {
        private readonly IOllamaClient _ollamaClient;
        private readonly IEmbeddingGenerator _embeddingGenerator;
        private readonly IEmbeddingsAccessor _embeddingsAccessor;
        private readonly IMassageHistoryLoader _massageHistoryLoader;

        public LlmService(IOllamaClient ollamaClient, IEmbeddingGenerator embeddingGenerator, IEmbeddingsAccessor embeddingsAccessor, IMassageHistoryLoader massageHistoryLoader)
        {
            _ollamaClient = ollamaClient;
            _embeddingGenerator = embeddingGenerator;
            _embeddingsAccessor = embeddingsAccessor;
            _massageHistoryLoader = massageHistoryLoader;
        }

        public Task<float[]> GetEmbeddingAsync(string text)
        {
            return _embeddingGenerator.GenerateAsync(text);
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            List<Message> history = _massageHistoryLoader.LoadHistoryAsync();

            string lastMessage = "";
            if (history.Count > 0)
            {
                lastMessage = history[history.Count - 1].Content;
            }
            var contexts = await _embeddingsAccessor.SearchAsync(prompt + lastMessage, 1);
            var response = await _ollamaClient.GenerateAsync("qwen3:0.6b", prompt, history, [.. contexts]);
            _massageHistoryLoader.AddMessage(response);
            return response.LlmMessage.Content;
        }

        public async Task<Task> SaveEmbeddingAsync(string source, string content)
        {
            
            var chunks = TextSplitter.Split(content);
            foreach (var chunk in chunks)
            {
                await _embeddingsAccessor.SaveAsync(source, chunk);
                // Console.WriteLine($"Saved chunk of size {chunk.Length} from source {chunk}");
                
            }
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<string>> SearchEmbeddingsAsync(string query, int topK = 5)
        {
            return _embeddingsAccessor.SearchAsync(query, topK);
        }

        
    }
}