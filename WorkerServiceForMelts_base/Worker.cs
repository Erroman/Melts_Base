namespace App.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //_logger.LogInformation("����� � �����, ����� ������!");
            //Console.WriteLine("����� � �����, ����� ������!");
            await Task.Delay(1000, stoppingToken);
        }
    }
}