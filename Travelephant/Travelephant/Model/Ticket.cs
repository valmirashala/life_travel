using Microsoft.EntityFrameworkCore;

namespace Travelephant.Model
{
    public class Ticket
    {
        public int UserID { get; set; }
        
        public int BusID { get; set; }
        public bool IsActive { get; set; }
        public virtual BusInfo Bus { get; set; }
        public virtual User User { get; set; }
    }
}
