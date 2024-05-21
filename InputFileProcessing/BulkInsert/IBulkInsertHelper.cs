using InputFileProcessing.Models;

namespace InputFileProcessing.BulkInsert
{
    public interface IBulkInsertHelper
    {
        Task BulkInsert(List<TaxiTripFileEntity> data);
    }
}