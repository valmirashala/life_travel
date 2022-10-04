namespace Travelephant.Model
{
    public class BusInfo
    {
        public int BusId { get; set; }
        public string Name { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int TotalSeat { get; set; }
        public int AvailableSeat { get; set; }
        public int Price { get; set; }

    }
}
