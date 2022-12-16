using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using App.WorkerService;


IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = ".NET Joke Service -> Пошёл в пизду, мудак ебаный!";
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
