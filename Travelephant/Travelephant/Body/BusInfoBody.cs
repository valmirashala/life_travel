using System.ComponentModel.DataAnnotations;

namespace Travelephant.Body
{
    public class BusInfoBody
    {
        public int BusId { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [Required]
        [StringLength(20)]
        public string Departure { get; set; }
        [Required]
        [StringLength(20)]
        public string Destination { get; set; }
        [Required]
        public int DepartureTime { get; set; }
        [Required]
        public int TotalSeat { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
