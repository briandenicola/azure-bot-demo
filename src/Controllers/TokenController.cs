using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.ComposerBot.Json
{
    [Route("api/tokens")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private static IConfiguration _configuration;
        private readonly ILogger<string> _logger;

        public TokenController(IConfiguration configuration, ILogger<string> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var secret = _configuration["WebChannelSecret"];
            HttpClient client = new HttpClient(); 
            HttpRequestMessage request = new HttpRequestMessage( 
                HttpMethod.Get, 
                $"https://webchat.botframework.com/api/tokens");
 
            request.Headers.Authorization = new AuthenticationHeaderValue("BotConnector", secret);           
            var response = await client.SendAsync(request); 

            var token = string.Empty;
            if (response.IsSuccessStatusCode) 
            { 
                token = await response.Content.ReadAsStringAsync(); 
            } 
            
            return token;
        }
    }
}