using FileHelpers;
using InputFileProcessing.Convertation;

namespace InputFileProcessing.Models
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class TaxiTripDbEntity
    {
        [FieldOrder(2)]
        [FieldTrim(TrimMode.Both)]
        [FieldConverter(typeof(DateTimeConverter))]
        public DateTime tpep_pickup_datetime { get; set; }

        [FieldOrder(3)]
        [FieldTrim(TrimMode.Both)]
        [FieldConverter(typeof(DateTimeConverter))]
        public DateTime tpep_dropoff_datetime { get; set; }

        [FieldOrder(4)]
        [FieldTrim(TrimMode.Both)]
        [FieldConverter(typeof(NullableIntConverter))]
        public int? passenger_count { get; set; }

        [FieldOrder(5)]
        [FieldTrim(TrimMode.Both)]
        public decimal trip_distance { get; set; }

        [FieldOrder(7)]
        [FieldConverter(typeof(BoolConverter))]
        public bool? store_and_fwd_flag { get; set; }

        [FieldOrder(8)]
        [FieldTrim(TrimMode.Both)]
        public int PULocationID { get; set; }

        [FieldOrder(9)]
        [FieldTrim(TrimMode.Both)]
        public int DOLocationID { get; set; }

        [FieldOrder(11)]
        [FieldTrim(TrimMode.Both)]
        public decimal fare_amount { get; set; }

        [FieldOrder(14)]
        [FieldTrim(TrimMode.Both)]
        public decimal tip_amount { get; set; }
    }
}
