using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guidex_Backend.Infrastructure.Prompts;

namespace Guidex_Backend.Infrastructure.Responses
{
    public class LlmResponse
    {
        public Message UserMessage { get; set; } = new Message("user", "");
        public Message LlmMessage { get; set; } = new Message("assistant", "");
    }
}