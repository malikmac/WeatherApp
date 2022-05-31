using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Text;
using WeatherApp.ApiService;
using WeatherApp.ViewModels;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOpenMeteoApi _openMeteoApi;

        public HomeController(ILogger<HomeController> logger, IOpenMeteoApi openMeteoApi)
        {
            _logger = logger;
            _openMeteoApi = openMeteoApi;
        }

        [HttpGet("/", Name = "Home")]
        public IActionResult Index()
        {
            var model = new IndexViewModel();

            return View(model);
        }

        [HttpPost("/", Name = "Home")]
        public async Task<IActionResult> Index(
            [FromForm] IndexViewModel model,
            CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _openMeteoApi.GetForcast(model.Latitude, model.Longitude);
            if (result.Succeeded)
            {
                model.AverageTemperatures = _openMeteoApi.GetAverageTemperatures(result.Result);
                TempData["AverageTemperatures"] = model.AverageTemperatures;

                return View(model);
            }

            foreach (var error in result.Errors)
                ModelState.TryAddModelError(string.Empty, error.Value);

            return View(model);
        }

        [HttpGet("/download-result", Name = "DownloadResult")]
        public async Task<FileStreamResult> DownloadResult()
        {
            var result = string.Empty;

            if (TempData["AverageTemperatures"] != null)
            {
                result += "day, temperature\n";
                foreach (var item in TempData["AverageTemperatures"] as Dictionary<string, string>)
                    result += $"{item.Key}, {item.Value.Replace(',', '.')}\n";
            }

            var stream = new MemoryStream(Encoding.ASCII.GetBytes(result));
            return new FileStreamResult(stream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = "average_temperatures.txt"
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}