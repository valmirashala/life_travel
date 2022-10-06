using System.ComponentModel.DataAnnotations;

namespace Travelephant.Body
{
    public class UserBody
    {

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [StringLength(20)]
        public string Surname { get; set; }
        [Required]
        [StringLength(20)]
        public string Username { get; set; }
        [Required]
        [StringLength(20)]
        public string? Address { get; set; }
    }
}
