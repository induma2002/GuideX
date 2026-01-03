using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guidex_Backend.Util
{
    public static class TextSplitter
    {
        private static readonly Regex SentenceRegex =
            new Regex(@"(?<=[.!?])\s+", RegexOptions.Compiled);

        public static List<string> Split(
            string text,
            int maxWords = 400,
            int overlap = 80)
        {
            var sentences = SentenceRegex.Split(text);
            var chunks = new List<string>();
            var currentWords = new List<string>();

            foreach (var sentence in sentences)
            {
                var words = sentence
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                currentWords.AddRange(words);

                if (currentWords.Count >= maxWords)
                {
                    chunks.Add(string.Join(" ", currentWords));

                    // keep overlap
                    currentWords = currentWords
                        .Skip(Math.Max(0, currentWords.Count - overlap))
                        .ToList();
                }
            }

            if (currentWords.Count > 0)
            {
                chunks.Add(string.Join(" ", currentWords));
            }

            return chunks;
        }
    }
}