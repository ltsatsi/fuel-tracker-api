using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedOn { get; set; }


        public bool IsActive { get; set; }  
    } // end class
} // end namespace