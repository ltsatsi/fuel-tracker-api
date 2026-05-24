using System.ComponentModel.DataAnnotations.Schema;

namespace Domain_Layer.Models
{
    public class Vehicle : BaseEntity
    {
        public string Registration { get; set; }
        public string Image { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }


        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? ApplicationUser { get; set; }    
        public ICollection<Fuel> FuelRecords { get; set; } = new List<Fuel>();
    } // end class
} // end namespace
