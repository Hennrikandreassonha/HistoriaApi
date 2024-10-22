using HistoriaApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace HistoriaApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HistoryController : ControllerBase
{
    private readonly FileService _fileService;
    public HistoryController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPut]
    public IActionResult Put([FromBody] HistoryInput input)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (!_fileService.IsHistorySubject(input.Subject))
        {
            _fileService.AddSubject("../NotHistoric.txt", input);
            return BadRequest("Not an historic event or subject");
        }

        _fileService.AddSubject("../AiWeeklySubjects.txt", input);

        return Ok("Subject added");
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Subject added");
    }
}