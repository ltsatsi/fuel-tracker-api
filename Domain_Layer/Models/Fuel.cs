namespace Domain_Layer.Models
{
    public class Fuel : BaseEntity
    {
        public double Price { get; set; }
        public string Station { get; set; }
        public double Liter { get; set; }
        public double Odometer { get; set; }


        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }   
    } // end class
} // end namespace
