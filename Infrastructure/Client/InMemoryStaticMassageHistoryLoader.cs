using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guidex_Backend.Infrastructure.Interface;
using Guidex_Backend.Infrastructure.Prompts;
using Guidex_Backend.Infrastructure.Responses;

namespace Guidex_Backend.Infrastructure.Client
{
    public class InMemoryStaticMassageHistoryLoader : IMassageHistoryLoader
    {
        public List<Message> LoadHistoryAsync()
        {
            return PromptHistory.History;
        }
        public void AddMessage(Message message)
        {
            PromptHistory.History.Add(message);
        }
        public void SecureLastNMessages(int n)
        {
            var history = PromptHistory.History;

            if (history.Count <= n)
                return;

            int removeCount = history.Count - n;
            history.RemoveRange(0, removeCount); // remove oldest
        }

        public void AddMessage(LlmResponse message)
        {
            PromptHistory.History.Add(message.UserMessage);
            PromptHistory.History.Add(message.LlmMessage);
        }
    }
}