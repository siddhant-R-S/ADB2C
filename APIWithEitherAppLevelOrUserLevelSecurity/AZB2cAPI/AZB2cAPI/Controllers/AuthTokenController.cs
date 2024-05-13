using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AZB2cAPI.Controllers
{
    [ApiController]
    public class AuthTokenController : ControllerBase
    {

        [HttpGet]
        [Route("GetToken/clientId/{clientId}/clientSecret/{clientSecret}")]
        public async Task<IActionResult> GetToken([FromRoute] string clientId, [FromRoute] string clientSecret)
        {
            string accessToken = string.Empty;
            var data = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type","client_credentials"),
                new KeyValuePair<string, string>("scope","https://prismhrwfm.onmicrosoft.com/secureid/api/.default"),
                new KeyValuePair<string, string>("client_id",clientId),
                new KeyValuePair<string, string>("client_secret",clientSecret)
            };
            using (var httpClient = new HttpClient())
            {
                using (var content = new FormUrlEncodedContent(data))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.PostAsync("https://prismhrwfm.b2clogin.com/prismhrwfm.onmicrosoft.com/B2C_1_SecureID-Universal-Signinup-flow/oauth2/v2.0/token", content);

                    var rr = await response.Content.ReadAsStringAsync();
                    var tokendetails = JsonConvert.DeserializeObject<TokenDetails>(rr);
                    accessToken = tokendetails.access_token;
                }
            }
            return Ok(accessToken);
        }
    }
    public class TokenDetails
    {
        public String access_token { get; set; }
        public String token_type { get; set; }
        public String expires_in { get; set; }

    }
}
