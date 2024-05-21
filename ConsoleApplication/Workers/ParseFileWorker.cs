using ConsoleApplication;
using FileHelpers;
using InputFileProcessing.BulkInsert;
using InputFileProcessing.Models;
using System.Reflection.Metadata.Ecma335;

namespace WorkerApplication.Workers
{
    public class ParseFileWorker : IParseFileWorker
    {
        private readonly ILogger<Worker> Logger;
        private readonly IBulkInsertHelper BulkInsertHelper;
        private readonly string FileDirectory;
        private readonly List<TaxiTripFileEntity> AllTrips;
        private readonly List<TaxiTripFileEntity> UniqueTrips;
        private readonly List<TaxiTripFileEntity> DuplicatedTrips;

        public ParseFileWorker(ILogger<Worker> logger, IBulkInsertHelper repository, IConfiguration configuration)
        {
            this.Logger = logger;
            this.BulkInsertHelper = repository;
            this.FileDirectory = configuration.GetValue<string>("FileDirectory");
            this.AllTrips = new();
            this.UniqueTrips = new();
            this.DuplicatedTrips = new();
        }

        public async Task ParseCsvFiles()
        {
            var engine = new FileHelperEngine<TaxiTripFileEntity>
            {
                Options = { IgnoreFirstLines = 1 }
            };

            try
            {
                var csvFiles = GetCsvFilesFromDirectory(FileDirectory);

                foreach (var file in csvFiles)
                {
                    var records = engine.ReadFile(file);

                    foreach (var record in records)
                    {
                        this.AllTrips.Add(record);
                    }

                    IdentifyTrips();

                    this.Logger.LogInformation($"allTrips.Count = {AllTrips.Count}, uniqueTrips = {UniqueTrips.Count}, duplicatedTrips = {DuplicatedTrips.Count}");

                    await BulkInsertHelper.BulkInsert(UniqueTrips);

                    var duplicatesFolderPath = Path.Combine(FileDirectory, "Duplicates");
                    if(!Directory.Exists(duplicatesFolderPath))
                        Directory.CreateDirectory(duplicatesFolderPath);

                    var handledUniqueTripsFolderPath = Path.Combine(FileDirectory, "Handled");
                    if (!Directory.Exists(handledUniqueTripsFolderPath))
                        Directory.CreateDirectory(handledUniqueTripsFolderPath);

                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                    await WriteDuplicateDataToCsvFile(this.DuplicatedTrips, Path.Combine(duplicatesFolderPath, $"{fileNameWithoutExtension}_duplicates.csv"));
                    var handledFileName = $"{handledUniqueTripsFolderPath}/{fileNameWithoutExtension}_handled.csv";

                    if (File.Exists(handledFileName))
                        handledFileName = this.MakeNameUnique(handledFileName);
                    File.Move(file, handledFileName);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"ParseCsvFile error: {ex.Message}");
            }
        }
        private async Task WriteDuplicateDataToCsvFile(List<TaxiTripFileEntity> trips, string filePath)
        {
            var engine = new FileHelperEngine<TaxiTripFileEntity>();

            try
            {
                if (File.Exists(filePath))
                    filePath = this.MakeNameUnique(filePath);
                engine.WriteFile(filePath, trips);
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"Error writing data to CSV: {ex.Message}");
            }
        }
        private void IdentifyTrips()
        {
            this.UniqueTrips.Clear();
            this.DuplicatedTrips.Clear();

            var tripGroups = this.AllTrips.GroupBy(t => new { t.tpep_pickup_datetime, t.tpep_dropoff_datetime, t.passenger_count });

            foreach (var group in tripGroups)
            {
                bool isFirstOccurance = true;
                foreach (var trip in group)
                {
                    if (isFirstOccurance)
                    {
                        this.UniqueTrips.Add(trip);
                        isFirstOccurance = false;
                    }
                    else
                    {
                        this.DuplicatedTrips.Add(trip);
                    }
                }
            }
        }
        private List<string> GetCsvFilesFromDirectory(string directoryPath)
        {
            var csvFiles = new List<string>();

            try
            {
                if (Directory.Exists(directoryPath))
                {
                    var files = Directory.GetFiles(directoryPath, "*.csv", SearchOption.TopDirectoryOnly);
                    csvFiles.AddRange(files);
                }
                else
                {
                    this.Logger.LogInformation($"The directory {directoryPath} does not exist.");
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogInformation($"An error occurred while retrieving CSV files: {ex.Message}");
            }

            return csvFiles;
        }
        private string MakeNameUnique(string filename)
        {
            var parts = filename.Split('.');
            return parts[0] + DateTime.UtcNow.ToString("ddMMyyyyhhmmss") + "." + parts[1];
        }
    }
}
