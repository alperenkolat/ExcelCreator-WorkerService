using ExcelCreatorWorkerService;
using ExcelCreatorWorkerService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext,services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

        services.AddSingleton<RabbitMQClientService>();
        services.AddSingleton(sp => new ConnectionFactory()
        {
            Uri = new Uri(
                configuration.GetConnectionString("RabbitMQ")),
            DispatchConsumersAsync = true
        });
    })
    .Build();

await host.RunAsync();
