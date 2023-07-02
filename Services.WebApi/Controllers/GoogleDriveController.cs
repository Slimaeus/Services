using Google;
using Google.Apis.Drive.v2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.WebApi.Configurations;

namespace Services.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GoogleDriveController : ControllerBase
{
    protected IHttpClientFactory HttpClientFactory => HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
    public IOptions<GoogleDriveSettings> Options => HttpContext.RequestServices.GetRequiredService<IOptions<GoogleDriveSettings>>();
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var apiKey = Options.Value.Key;
        var fileId = id;
        //var client = HttpClientFactory.CreateClient("Drive");
        //var result = await client.GetAsync($"https://www.googleapis.com/drive/v2/files/{fileId}?key={apiKey}");
        //var file = JsonConvert.DeserializeObject<File>(await result.Content.ReadAsStringAsync());

        var initializer = new Google.Apis.Services.BaseClientService.Initializer
        {
            ApiKey = apiKey
        };
        var driveService = new DriveService(initializer);
        var request = driveService.Files.Get(fileId);
        try
        {
            var file = await request.ExecuteAsync();
            return Ok(file);
        }
        catch (GoogleApiException exception)
        {

            return BadRequest(exception.Message);
        }

    }
    [HttpGet("{id}/with-items")]
    public async Task<IActionResult> GetWithItems(string id)
    {
        var apiKey = Options.Value.Key;
        var fileId = id;
        var client = HttpClientFactory.CreateClient("Drive");
        var result = await client.GetAsync($"https://www.googleapis.com/drive/v2/files?q='{fileId}'+in+parents&key={apiKey}");
        var file = JsonConvert.DeserializeObject<Google.Apis.Drive.v2.Data.FileList>(await result.Content.ReadAsStringAsync());

        return Ok(file);
    }
}
