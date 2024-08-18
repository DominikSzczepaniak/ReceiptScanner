namespace Backend.Data;

public class DatabaseInitializer : IHostedService
{
    private readonly IDatabaseHandler _databaseHandler;

    public DatabaseInitializer(IDatabaseHandler databaseHandler)
    {
        _databaseHandler = databaseHandler;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _databaseHandler.ConnectAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}