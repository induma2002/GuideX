using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guidex_Backend.Application.Interface
{
    public interface ILlmService
    {
        Task<string> GetResponseAsync(string prompt);
        Task<float[]> GetEmbeddingAsync(string text);
        Task<Task> SaveEmbeddingAsync(string source, string content);
        Task<IReadOnlyList<string>> SearchEmbeddingsAsync(string query, int topK = 5);
    }
}