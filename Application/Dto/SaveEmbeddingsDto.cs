using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guidex_Backend.Application.Dto
{
    public class SaveEmbeddingsDto
    {
        public string Source { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}