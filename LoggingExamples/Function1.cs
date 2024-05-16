using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LoggingExamples
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("This is my Informational Test");
            _logger.LogWarning("This is my Warning Test");
            _logger.LogError("This is my Error Test");
            _logger.LogCritical("This is my Critical Test");
            
            return new OkObjectResult("Hello World!");
        }
    }
}
