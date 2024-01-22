using System.ComponentModel.DataAnnotations;

namespace CustomerAPI.Dtos.Customer
{
    public class CustomerInsertDto
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        public string Email { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
    }
}
