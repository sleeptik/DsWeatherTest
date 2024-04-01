using Microsoft.AspNetCore.Mvc;
using WebApi.Archive;
using WebApi.Domain.Models;
using WebApi.Services;
using WebApi.Utility;

namespace WebApi.Controllers;

[ApiController]
[Route("api/archives")]
public class ArchiveController(ArchiveService archiveService) : ControllerBase
{
    [HttpGet("")]
    public IActionResult GetWeatherRecords([FromQuery] string date)
    {
        var dateOnly = DateOnly.ParseExact(date, "yyyy-MM");
        var dateTime = ArchiveTimeConverter.MoscowToUtc(dateOnly.ToDateTime(TimeOnly.MinValue));
        var recordByYearMonth = archiveService.GetRecordByYearMonth(dateTime);
        return Ok(recordByYearMonth);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadArchives()
    {
        if (!Request.Form.Files
                .All(file => file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
        {
            ModelState.AddModelError("files", "Only XLSX files are supported.");
            return BadRequest(ModelState);
        }

        var recordsFromAllFiles = new List<WeatherRecord>();
        var processTasks = new List<Task>(Request.Form.Files.Count);

        processTasks.AddRange(Request.Form.Files.Select(formFile => Task.Run(async () =>
        {
            await using var stream = formFile.OpenReadStream();
            using var archiveReader = new XlsxArchiveReader(stream);

            var recordsFromFile = archiveReader.Foo();

            lock (recordsFromAllFiles)
            {
                recordsFromAllFiles.AddRange(recordsFromFile);
            }
        })));

        await Task.WhenAll(processTasks);

        if (recordsFromAllFiles.Count > 0)
            archiveService.SaveRecords(recordsFromAllFiles);

        return NoContent();
    }
}