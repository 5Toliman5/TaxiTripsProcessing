using InputFileProcessing.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace InputFileProcessing.BulkInsert
{
    public class BulkInsertHelper : IBulkInsertHelper
    {
        private string ConnectionString { get; }

        public BulkInsertHelper(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("default");
        }

        public async Task BulkInsert(List<TaxiTripFileEntity> data)
        {
            var dataTable = this.CreateDataTable();
            foreach (var item in data)
            {
                dataTable.Rows.Add(
                    item.tpep_pickup_datetime,
                    item.tpep_dropoff_datetime,
                    item.passenger_count,
                    item.trip_distance,
                    item.store_and_fwd_flag,
                    item.PULocationID,
                    item.DOLocationID,
                    item.fare_amount,
                    item.tip_amount
                );
            }

            using var connection = new SqlConnection(this.ConnectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                {
                    this.PrepareBulkCopy(bulkCopy);
                    await bulkCopy.WriteToServerAsync(dataTable);
                }
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        private DataTable CreateDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("tpep_pickup_datetime", typeof(DateTime));
            dataTable.Columns.Add("tpep_dropoff_datetime", typeof(DateTime));
            dataTable.Columns.Add("passenger_count", typeof(int));
            dataTable.Columns.Add("trip_distance", typeof(decimal));
            dataTable.Columns.Add("store_and_fwd_flag", typeof(string));
            dataTable.Columns.Add("PULocationID", typeof(int));
            dataTable.Columns.Add("DOLocationID", typeof(int));
            dataTable.Columns.Add("fare_amount", typeof(decimal));
            dataTable.Columns.Add("tip_amount", typeof(decimal));
            return dataTable;
        }
        private void PrepareBulkCopy(SqlBulkCopy bulkCopy)
        {
            bulkCopy.DestinationTableName = "TaxiTrips";
            bulkCopy.ColumnMappings.Add("tpep_pickup_datetime", "tpep_pickup_datetime");
            bulkCopy.ColumnMappings.Add("tpep_dropoff_datetime", "tpep_dropoff_datetime");
            bulkCopy.ColumnMappings.Add("passenger_count", "passenger_count");
            bulkCopy.ColumnMappings.Add("trip_distance", "trip_distance");
            bulkCopy.ColumnMappings.Add("store_and_fwd_flag", "store_and_fwd_flag");
            bulkCopy.ColumnMappings.Add("PULocationID", "PULocationID");
            bulkCopy.ColumnMappings.Add("DOLocationID", "DOLocationID");
            bulkCopy.ColumnMappings.Add("fare_amount", "fare_amount");
            bulkCopy.ColumnMappings.Add("tip_amount", "tip_amount");
        }
    }
}
