using System;
using System.IO;
using System.Threading.Tasks;
using FileImport.Api.Contracts.Requests;
using FileImport.Api.Mapping;
using FileImport.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileImport.Api.Controllers
{
    [ApiController]
    [Route("import")]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ImportRequest request)
        {
            var importRequest = request.MapToImportRequest();
            var result = await _importService.Import(importRequest.InputLocation, importRequest.OutputLocation);
            if (result.Success)
            {
                return Accepted();
            }
            else return Problem(result.Message);
        }
        
        [HttpGet("confirm")]
        public async Task<IActionResult> Confirm(string location)
        {
            var text = "";
            var readLocation = $"{Directory.GetCurrentDirectory()}";
            var entireReadLocation = $"{readLocation}{location}";

            try
            {
                using (var sr = new StreamReader(entireReadLocation))
                {
                    text = await sr.ReadToEndAsync();
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            
            return Accepted(text);
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Accepted();
        }
    }
}