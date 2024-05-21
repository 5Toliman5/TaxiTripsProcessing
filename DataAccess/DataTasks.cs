using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
    public class DataTasks : IDataTasks
    {
        private CabFaresProcessingContext DbContext;
        public DataTasks(IConfiguration configuration)
        {
            this.DbContext = new(configuration.GetConnectionString("default"));
        }
        public async Task<int> GetTheMostTippedLocationAsync()
        {
            return await this.DbContext.TaxiTrips
            .GroupBy(x => x.PulocationId)
            .Select(g => new
            {
                PULocationId = g.Key,
                AverageTipAmount = g.Average(x => x.TipAmount)
            })
            .OrderByDescending(x => x.AverageTipAmount)
            .Select(x => x.PULocationId)
            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TaxiTrip>> GetTheLongestTripsByDistance(int quantity)
        {
            return await this.DbContext.TaxiTrips
            .OrderByDescending(x => x.TripDistance)
            .Take(quantity)
            .ToListAsync();
        }
        public async Task<IEnumerable<TaxiTrip>> GetTheLongestTripsByTime(int quantity)
        {
            return await this.DbContext.TaxiTrips.FromSqlRaw($"SELECT TOP {quantity} * FROM TaxiTrips ORDER BY DATEDIFF(SECOND, tpep_pickup_datetime, tpep_dropoff_datetime) DESC").ToListAsync();
        }
    }
}
