using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Driver;
using GodOfPicsAPI.Models;
using GodOfPicsAPI.Services;
using Newtonsoft.Json;


namespace GodOfPicsAPI.Controllers
{
    [Controller]
    [Route("api/[controller]")]

    public class PhotosController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly MongoDBService _mongoDBService;
        private readonly string _unsplashAccessKey;

        public PhotosController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService; 
            _unsplashAccessKey = "kxn18J2DrvVq8BrasEWUY7F3WHazG8SNN5Y3b3INrEo";
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept-Version", "v1");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Client-ID {_unsplashAccessKey}");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePhoto(string id)
        {
            var deletedPhoto = await _mongoDBService.DeleteByIdAsync(id);
            if (deletedPhoto == null)
            {
                return NotFound();
            }

            return Ok(deletedPhoto);
        }
        [HttpPost("save")]
        public async Task<IActionResult> SavePhoto([FromBody] Photos photo)
        {
            await _mongoDBService.CreateAsync(photo);
            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPhotos([FromQuery] string query)
        {
            var response = await _httpClient.GetAsync($"https://api.unsplash.com/photos/random?query={query}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return Ok(content);
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomPhoto()
        {
            var response = await _httpClient.GetAsync("https://api.unsplash.com/photos/random");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpGet("best")]
        public async Task<IActionResult> GetBestPhoto()
        {
            var response = await _httpClient.GetAsync("https://api.unsplash.com/photos/random?order_by=popular&per_page=1");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }  
    }
}
