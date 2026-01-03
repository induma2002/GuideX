using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guidex_Backend.Infrastructure.Prompts
{
    public static class PromptHistory
    {
        public static List<Message> History { get; set; } = new();
    }
}