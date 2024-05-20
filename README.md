# Managing Log Levels in Azure Functions With Isolated Worker

## Important Considerations

When working with Azure Functions and **isolated process workers**, it is crucial to understand that the Functions host and the isolated process worker maintain separate configurations for log levels and other settings. Configurations made in `host.json` for Application Insights will not impact the logging behavior of the worker, and vice versa. To ensure proper logging across both the host and the worker, configurations must be applied at both layers.

## Configuration with ILogger and ILogger<T>

Despite these separate configurations, your application will still utilize `ILogger` and `ILogger<T>`. By default, the Application Insights SDK applies a logging filter that only captures warnings and more severe logs. If you need to capture more detailed logs or modify this behavior, you will need to adjust the filter rule within your service configuration.

### Example Configuration - Informational Level

Below is a sample code snippet showing how to modify the logging settings in an Azure Functions project using an isolated worker. This example demonstrates removing the default filter rule that limits logging to warnings and above.

#### program.cs
```csharp
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .ConfigureLogging(logging =>
    {
        logging.Services.Configure<LoggerFilterOptions>(options =>
        {
            LoggerFilterRule defaultRule = options.Rules.FirstOrDefault(rule => rule.ProviderName
                == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
            if (defaultRule is not null)
            {
                options.Rules.Remove(defaultRule);
            }
        });
    })
    .Build();

host.Run();
```

#### host.json 

```json
{
  "version": "2.0",
  "logging": {
    "applicationInsights": {
      "samplingSettings": {
        "isEnabled": true,
        "excludedTypes": "Request"
      },
      "enableLiveMetricsFilters": true
    }
  }
}
```

## Testing Locally

To ensure all logging works before deployment you can alter local.settings.json to include `APPINSIGHTS_INSTRUMENTATIONKEY`. This key can be found inside Azure Portal > Application Insights > Configure > Properties > INSTRUMENTATION KEY

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "APPINSIGHTS_INSTRUMENTATIONKEY": "{Insert key here}"
  }
}
```

## Viewing live telemetry data
When test locally or within a deployed instance you will be able to see live telemetry data. Azure Portal > Application Insights > Live Metrics 

![sample_telemetry](https://github.com/Al3xDaniels/AzFuncLogging/assets/8877576/9a427466-938a-452b-89ac-48269c929cb6)

## Looking at the logs

Using this Kusto query we are able to see the logs for Informational and above. 

```SQL
union isfuzzy=true traces
| where message contains "This is my"
| order by timestamp desc
| take 100
```

![insights_logs](https://github.com/Al3xDaniels/AzFuncLogging/assets/8877576/23cf6d33-977e-456f-a679-76b852b05a97)

## Helpful links
 - [Managing log levels](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=windows#managing-log-levels)




