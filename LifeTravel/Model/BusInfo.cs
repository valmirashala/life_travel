namespace LifeTravel.Model
{
    public class BusInfo
    {
        public int Id { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AvailableSeats { get; set; }
        public int TotalSeats { get; set; }
        public int Price { get; set; }

    }
}
