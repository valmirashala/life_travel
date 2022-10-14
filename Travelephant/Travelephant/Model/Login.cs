using System.ComponentModel.DataAnnotations;

namespace Travelephant.Model
{
    public class Login
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string name { get; set; }
    }
}
