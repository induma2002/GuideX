using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guidex_Backend.Infrastructure.Interface
{
    public interface IEmbeddingsAccessor
{
    Task SaveAsync(string source, string content);
    Task<IReadOnlyList<string>> SearchAsync(string query, int topK = 5);
}

}