using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace AzureB2CTester.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<string> Get()
        {
            await PrepareAuthenticatedClient();
            var response = await _httpClient.GetAsync("https://localhost:7231/WeatherForecast");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
            return "failed";
        }
        private async Task PrepareAuthenticatedClient()
        {
            string accessToken = string.Empty;
            var data = new List<KeyValuePair<string, string>>()
            {
                // all of these below credentials should be moved to configurations and should be stored in azure keyvault
                new KeyValuePair<string, string>("grant_type","client_credentials"),
                new KeyValuePair<string, string>("scope","https://siddhantazb2c.onmicrosoft.com/secureid/api/.default"),
                new KeyValuePair<string, string>("client_id","cd25429d-94b2-4fa5-8c6e-9c4aa8df40f8"),
                new KeyValuePair<string, string>("client_secret","-298Q~NOBicGlm-lRud6ps.__1SGzMQb__")
            };
            using (var httpClient = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(data))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.PostAsync("https://siddhantazb2c.b2clogin.com/siddhantazb2c.onmicrosoft.com/B2C_1_SignUpSignIn/oauth2/v2.0/token", content);

                    var rr = await response.Content.ReadAsStringAsync();
                    var tokendetails = JsonConvert.DeserializeObject<TokenDetails>(rr);
                    accessToken = tokendetails.access_token;
                }
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
