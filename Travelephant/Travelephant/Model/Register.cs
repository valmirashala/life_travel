using System.ComponentModel.DataAnnotations;

namespace Travelephant.Model
{
    public class Register
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string confirmPassword { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string Surname { get; set; }
      
        [Required]
        public string? Address { get; set; }

        [Required]
        public bool IsAdmin { get; set; }
    }
}
