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
            _logger.LogCritical("CritTest", new { FunctionName = "MyHttpTrigger" });
            _logger.LogWarning("WarnTest", new { FunctionName = "MyHttpTrigger" });
            _logger.LogInformation("InfoTest", new { FunctionName = "MyHttpTrigger" });

            return new OkObjectResult("Hello World!");
        }
    }
}
