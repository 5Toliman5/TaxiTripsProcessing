using DataAccess;
using InputFileProcessing.BulkInsert;
using WorkerApplication.Workers;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddTransient<IBulkInsertHelper, BulkInsertHelper>();
            builder.Services.AddTransient<IParseFileWorker, ParseFileWorker>();
            builder.Services.AddTransient<IDisplayDataWorker, DisplayDataWorker>();
            builder.Services.AddTransient<IDataTasks, DataTasks>();
            var host = builder.Build();
            host.Run();
        }
    }
}