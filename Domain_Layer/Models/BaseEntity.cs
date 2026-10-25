using System.ComponentModel.DataAnnotations;

namespace Domain_Layer.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }


        public bool IsActive { get; set; }  
    } // end class
} // end namespace