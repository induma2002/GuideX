using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guidex_Backend.Application.Dto
{
    public class PdfUploadRequestDto
    {
        public IFormFile File { get; set; } = default!;
    }
}