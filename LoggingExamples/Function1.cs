using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LoggingExamples
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly TelemetryClient _telemetryClient;

        public Function1(ILogger<Function1> logger, TelemetryClient telemetryClient)
        {
            _logger = logger;
            _telemetryClient = telemetryClient;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            // Basic logging examples
            _logger.LogInformation("This is my Informational Test");
            _logger.LogWarning("This is my Warning Test");
            _logger.LogError("This is my Error Test");
            _logger.LogCritical("This is my Critical Test");


            // Created a custom dimension in the telemetry entry that is logged as a Trace
            _telemetryClient.TrackTrace("This is my TelemetryClient Test 3",
                               Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information,
                                              new System.Collections.Generic.Dictionary<string, string>
                                              { { "customDimension", "customDimensionValue" } });

            // Created a custom dimension in the telemetry entry that is logged as an CustomEvent with a custom dimension
            _telemetryClient.TrackEvent("This is my TelemetryClient Test 4",
                                                             new System.Collections.Generic.Dictionary<string, string>
                                                             { { "customDimension", "customDimensionValue" } });

            return new OkObjectResult("Hello World!");
        }
    }
}
