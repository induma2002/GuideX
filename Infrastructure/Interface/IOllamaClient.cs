using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guidex_Backend.Infrastructure.Prompts;
using Guidex_Backend.Infrastructure.Responses;

namespace Guidex_Backend.Infrastructure.Interface
{
    public interface IOllamaClient
    {
        Task<LlmResponse> GenerateAsync(string model, string messages, List<Message>? history = null, List<string>? contexts = null);
    }
}