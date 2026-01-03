using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guidex_Backend.Infrastructure.Interface
{
    public interface IEmbeddingGenerator
    {
      Task<float[]> GenerateAsync(string text);  
    }
}