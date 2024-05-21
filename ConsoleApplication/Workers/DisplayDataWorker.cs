using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerApplication.Workers
{
    public class DisplayDataWorker : IDisplayDataWorker
    {
        private readonly ILogger<DisplayDataWorker> Logger;
        private readonly IDataTasks DataTasks;

        public DisplayDataWorker(ILogger<DisplayDataWorker> logger, IDataTasks dataTasks)
        {
            this.Logger = logger;
            this.DataTasks = dataTasks;
        }
        public async Task DisplayData()
        {
            try
            {
                var selectDataQuantity = 100;
                //task 1
                var tippedLocationId = await this.DataTasks.GetTheMostTippedLocationAsync();
                Console.WriteLine($"\nPULocationId {tippedLocationId} has the highest tip_amount on average\n");
                //task 2
                var longestTripsByDistance = await this.DataTasks.GetTheLongestTripsByDistance(selectDataQuantity);
                Console.WriteLine($"\nTop {selectDataQuantity} longest trips in terms of distance:\n");
                foreach (var item in longestTripsByDistance)
                {
                    Console.WriteLine($"| pickup: {item.TpepPickupDatetime} | dropoff: {item.TpepDropoffDatetime} | distance:{item.TripDistance}");
                }
                //task 3
                var longestTripsByTime = await this.DataTasks.GetTheLongestTripsByTime(selectDataQuantity);
                Console.WriteLine($"\nTop {selectDataQuantity} longest trips in terms of distance:\n");
                foreach (var item in longestTripsByTime)
                {
                    Console.WriteLine($"| pickup: {item.TpepPickupDatetime} | dropoff: {item.TpepDropoffDatetime} | distance:{item.TripDistance}");
                }
            }
            catch(Exception ex)
            {
                this.Logger.LogError(ex.Message);
            }

        }

    }
}
