namespace BackgroundTasksWithTelegramBot.BackgroundTasks;

public class TestService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("TestService is running");

            await Task.Delay(3000, stoppingToken);
        }
    }
}
