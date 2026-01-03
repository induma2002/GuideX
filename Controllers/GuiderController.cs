using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guidex_Backend.Application.Dto;
using Microsoft.AspNetCore.Mvc;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Guidex_Backend.Controllers
{
    [ApiController]
    [Route("guider")]
    public class GuiderController : ControllerBase
    {
        private readonly Application.Interface.ILlmService _llmService;

        public GuiderController(Application.Interface.ILlmService llmService)
        {
            _llmService = llmService;
        }

        [HttpGet("hello")]
        public IActionResult GetHello()
        {
            return Ok("Hello from GuiderController!");
        }
        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] string prompt)
        {
            var response = await _llmService.GetResponseAsync(prompt);
            return Ok(response);
        }

        [HttpPost("embedding")]
        public async Task<IActionResult> GetEmbedding([FromBody] string text)
        {
            var embedding = await _llmService.GetEmbeddingAsync(text);
            return Ok(embedding);
        }

        [HttpPost("save-embedding")]
        public async Task<IActionResult> SaveEmbedding([FromBody] SaveEmbeddingsDto request)
        {
            await _llmService.SaveEmbeddingAsync(request.Source, request.Content);
            return Ok();
        }

        [HttpPost("search-embeddings")]
        public async Task<IActionResult> SearchEmbeddings([FromBody] SerchEmbeddingsDto request)
        {
            var results = await _llmService.SearchEmbeddingsAsync(request.Query, request.TopK);
            return Ok(results);
        }
        [HttpPost("upload-and-extract")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAndExtract([FromForm] PdfUploadRequestDto request)
        {
            var file = request.File;

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            if (!Path.GetExtension(file.FileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only PDF files allowed");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;

            var sb = new StringBuilder();

            using (var pdf = UglyToad.PdfPig.PdfDocument.Open(ms))
            {
                foreach (var page in pdf.GetPages())
                {
                    sb.AppendLine(page.Text);
                }
            }
            await _llmService.SaveEmbeddingAsync(file.FileName, sb.ToString());
        
            return Ok(new { Content = sb.ToString() });
        }

    }
}