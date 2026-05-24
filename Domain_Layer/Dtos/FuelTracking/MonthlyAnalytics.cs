namespace Domain_Layer.Dtos.FuelTracking
{
    public class MonthlyAnalytics
    {
        public int VehicleId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public double? TotalFuelCost { get; set; }
        public double? TotalDistanceCovered { get; set; }
        public double? TotalLitersPurchased { get; set; }

        public int? FuelEntryCount { get; set; }
    } // end class
} // end namespace
