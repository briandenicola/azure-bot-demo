using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Microsoft.BotBuilderSamples.Controllers
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
                $"https://webchat.botframework.com/api/tokens"
            );
 
            request.Headers.Authorization = new AuthenticationHeaderValue("BotConnector", secret);           
            var response = await client.SendAsync(request); 

            var token = String.Empty;
            if (response.IsSuccessStatusCode) 
            { 
                token = await response.Content.ReadAsStringAsync(); 
            } 
            
            return token;
        }
    }

}