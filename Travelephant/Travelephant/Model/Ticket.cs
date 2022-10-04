using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace Travelephant.Model
{
    public class Ticket
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public int BusID { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public virtual BusInfo Bus { get; set; }
        public virtual User User { get; set; }
    }
}
