using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guidex_Backend.Infrastructure.Model
{
    public sealed class EmbeddedDocument
    {
        public Guid Id { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public float[] Embedding { get; set; } = Array.Empty<float>();
    }
}