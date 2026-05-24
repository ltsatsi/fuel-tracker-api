namespace Domain_Layer.Dtos.FuelRecords
{
    public class FuelDto
    {
        public int Id { get; set; } 
        public double Price { get; set; }
        public string Station { get; set; }
        public double Liter { get; set; }
        public double Odometer { get; set; }
        public int VehicleId { get; set; }
    } // end class
} // end namespace
