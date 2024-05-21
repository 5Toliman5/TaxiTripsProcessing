using WorkerApplication.Workers;

namespace ConsoleApplication
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> Logger;
        private readonly IParseFileWorker ParseFileWorker;
        private readonly IDisplayDataWorker SelectDataWorker;

        public Worker(ILogger<Worker> logger, IParseFileWorker worker, IDisplayDataWorker selectDataWorker)
        {
            Logger = logger;
            this.ParseFileWorker = worker;
            this.SelectDataWorker = selectDataWorker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                if (Logger.IsEnabled(LogLevel.Information))
                {
                    Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await this.ParseFileWorker.ParseCsvFiles();
                await this.SelectDataWorker.DisplayData();
            }
        }
    }
}
