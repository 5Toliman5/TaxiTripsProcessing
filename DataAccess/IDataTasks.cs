
namespace DataAccess
{
    public interface IDataTasks
    {
        Task<IEnumerable<TaxiTrip>> GetTheLongestTripsByDistance(int quantity);
        Task<IEnumerable<TaxiTrip>> GetTheLongestTripsByTime(int quantity);
        Task<int> GetTheMostTippedLocationAsync();
    }
}