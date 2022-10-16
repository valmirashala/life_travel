using System.ComponentModel.DataAnnotations;

namespace Travelephant.Show
{
    public class BusInfoToShow
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Departure { get; set; }
        public int DepartureTime { get; set; }
        public string Destination { get; set; }
        public int ArrivalTime { get; set; }
        public double Price { get; set; }
        public int AvailableSeats { get; set; }
    }
}