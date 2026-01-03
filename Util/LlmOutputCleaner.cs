using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guidex_Backend.Util
{
    public static class LlmOutputCleaner
    {
        private static readonly Regex ThinkRegex =
            new Regex("<think>[\\s\\S]*?</think>", RegexOptions.IgnoreCase);

        public static string RemoveThinkBlocks(string text)
        {
            return ThinkRegex.Replace(text, "").Trim();
        }
    }
}