using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guidex_Backend.Infrastructure.Prompts;
using Guidex_Backend.Infrastructure.Responses;

namespace Guidex_Backend.Infrastructure.Interface
{
    public interface IMassageHistoryLoader
    {
        public List<Message> LoadHistoryAsync();
        public void AddMessage(Message message);
        public void AddMessage(LlmResponse message);
        public void SecureLastNMessages(int n);
    }
}