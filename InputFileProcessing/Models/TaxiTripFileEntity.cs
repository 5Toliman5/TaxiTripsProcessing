using FileHelpers;
using InputFileProcessing.Convertation;

namespace InputFileProcessing.Models
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class TaxiTripFileEntity : TaxiTripDbEntity
    {
        [FieldOrder(1)]
        [FieldConverter(typeof(NullableIntConverter))]
        public int? VendorID { get; set; }

        [FieldOrder(6)]
        [FieldConverter(typeof(NullableIntConverter))]
        public int? RatecodeID { get; set; }

        [FieldOrder(10)]
        public int? payment_type { get; set; }

        [FieldOrder(12)]
        public double? extra { get; set; }

        [FieldOrder(13)]
        public double? mta_tax { get; set; }

        [FieldOrder(15)]
        public double? tolls_amount { get; set; }

        [FieldOrder(16)]
        public double? improvement_surcharge { get; set; }

        [FieldOrder(17)]
        public double? total_amount { get; set; }

        [FieldOrder(18)]
        public double? congestion_surcharge { get; set; }
    }
}
