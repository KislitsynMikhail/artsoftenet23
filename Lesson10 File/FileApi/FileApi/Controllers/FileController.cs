using FileApi.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Stream = FileApi.Stream.Stream;

namespace FileApi.Controllers;

[ApiController]
[Route("file")]
public class FileController : ControllerBase
{
    private readonly IFileUseCaseService _fileUseCaseService;

    public FileController(IFileUseCaseService fileUseCaseService)
    {
        _fileUseCaseService = fileUseCaseService;
    }
    
    [HttpPost("create")]
    public IActionResult CreateFile([FromQuery] string fileContent)
    {
        _fileUseCaseService.CreateFile(fileContent);
        return Ok();
    }
    
    [HttpPost("create-async")]
    public async Task<IActionResult> CreateFileAsync([FromQuery] string fileContent)
    {
        await _fileUseCaseService.CreateFileAsync(fileContent);
        return Ok();
    }

    [HttpGet("get")]
    public async Task<FileStreamResult> GetFile()
    {
        return new FileStreamResult(await Stream.Stream.MemoryStreamExample(), MediaTypeHeaderValue.Parse("txt"));
    }

    [ProducesResponseType(typeof(FileStream), 200)]
    [HttpGet("get-async")]
    public async Task<FileStreamResult> GetFileAsync()
    {
        return File(await _fileUseCaseService.GetFileAsync(), "application/octet-stream", "text.txt");
    }
    
    [HttpPost("uploadFile")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file)
    {
        await _fileUseCaseService.SaveFile(file, "db");
        return Ok();
    }
}