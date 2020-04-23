#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");
    
    var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    log.LogInformation($"Request body: {requestBody}");
    
    return new OkObjectResult(new Status{ state="Password Reset"});
}

public class Status {
    public string state { get; set; }
}