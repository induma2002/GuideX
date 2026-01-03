using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guidex_Backend.Infrastructure.Interface;
using Guidex_Backend.Infrastructure.Responses;


namespace Guidex_Backend.Infrastructure.Client
{
    public sealed class EmbeddingGenerator384 : IEmbeddingGenerator
{
    private readonly HttpClient _http;

    public EmbeddingGenerator384(HttpClient http)
    {
        _http = http;
    }

    public async Task<float[]> GenerateAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Text cannot be empty");

        var response = await _http.PostAsJsonAsync(
            "http://localhost:11434/api/embeddings",
            new
            {
                model = "nomic-embed-text",
                prompt = text
            }
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<OllamaEmbeddingResponse>();

        if (result is null || result.Embedding.Length == 0)
            throw new InvalidOperationException("Failed to generate embedding");

        return result.Embedding;
    }
}

}