using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travelephant.Model
{
    public class Ticket
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
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
