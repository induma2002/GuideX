using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guidex_Backend.Application.Dto
{
    public class SerchEmbeddingsDto
    {
        public string Query { get; set; } = string.Empty;
        public int TopK { get; set; } = 5;
    }
}