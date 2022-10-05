using System.ComponentModel.DataAnnotations;

namespace Travelephant.Model
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }
        [MaxLength(20)]
        public string? Address { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
    }
}
