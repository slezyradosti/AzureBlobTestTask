using FunctionApp.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddOptions<SmtpData>()
        .Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection("SmtpGmailSecurity").Bind(settings);
        });
    })
    .Build();

host.Run();
