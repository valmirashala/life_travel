using Microsoft.AspNetCore.Mvc;
using Travelephant.Data;
using Travelephant.Model;

namespace Travelephant.Controllers
{
    public class BusInfoController : Controller
    {
        private readonly TravelephantContext _context;

        public BusInfoController(TravelephantContext context)
        {
            _context = context;
        }

        [HttpGet("all-bus-info")]
        public List<BusInfo> Get()
        {
            var allBusInfo = _context.BusInfo.ToList();
            return allBusInfo;
        }

        [HttpGet("sepcific-bus-info")]
        public IEnumerable<BusInfo> Get(string Departure, string Destination)
        {
            //Filters lines that satart at Departure and ends on Destination
            //Direct lines from Departure to Destination
            var FilteredData = _context.BusInfo
                .Where(x => x.Departure == Departure && x.Destination == Destination).ToList();

            //If there is no direct line
            if (FilteredData.Count == 0)
            {
                //Filter all lines that starts from StartCiti
                var FirstFilter = _context.BusInfo
                    .Where(x => x.Departure == Departure).ToList();

                //Filter all lines that ends on Destination
                var SecondFilter = _context.BusInfo
                    .Where(x => x.Destination == Destination).ToList();

                var FirstFiltered = new List<BusInfo>();
                var SecondFiltered = new List<BusInfo>();

                foreach (var item in FirstFilter)
                {
                    //Takes only lines that ends on Destination and start from the Destination
                    //of lines that start from Departure
                    var tmp = SecondFilter.Where(x => x.Departure == item.Destination).ToList();
                    if (tmp.Count != 0)
                    {
                        FirstFiltered = new List<BusInfo>(tmp);
                    }
                }

                foreach (var item in SecondFilter)
                {
                    //Takes only lines that starts on Departure and end on Departure
                    //of lines that ends on Destination
                    var tmp = FirstFilter.Where(x => x.Destination == item.Departure).ToList();
                    if (tmp.Count != 0)
                    {
                        SecondFiltered = new List<BusInfo>(tmp);
                    }
                }

                return SecondFiltered.Union(FirstFiltered);
            }

            return FilteredData;
        }

        [HttpGet("bus-line-id")]
        public int GetBusLineID(string Name, DateTime DepartureTime)
        {
            var busInfo = _context.BusInfo
                .Where(x => x.Name == Name && x.DepartureTime == DepartureTime).FirstOrDefault();

            return busInfo.Id;
        }

        [HttpPut("set-price")]
        public IEnumerable<BusInfo> SetPrice(string Username, string Name,
            DateTime DepartureTime, double Price)
        {
            //Get User with username == Username
            var user = _context.User
                .Where(x => x.Username == Username).FirstOrDefault();

            if (user.IsAdmin)
            {
                var busInfo = _context.BusInfo
                    .Where(x => x.Name == Name && x.DepartureTime == DepartureTime).FirstOrDefault();

                busInfo.Price = Price;

                _context.SaveChanges();
                var BusInfos = _context.BusInfo
                    .Where(x => x.Id == busInfo.Id).ToList();
                return BusInfos;
            }
            //If the user is not admin don't set price
            else
            {
                var BusInfos = _context.BusInfo
                    .Where(x => x.Name == Name && x.DepartureTime == DepartureTime).ToList();
                return BusInfos;
            }
        }

        [HttpPost("add-bus-line")]
        public IEnumerable<BusInfo> AddBusLine(string Username, string Name, string Departure,
            string Destination, DateTime DepartureTime, int TotalSeat, int Price)
        {
            //Get User with username == Username
            var user = _context.User
                .Where(x => x.Username == Username).FirstOrDefault();

            //If the user is admin add the new line
            if (user.IsAdmin)
            {
                var busInfo = new BusInfo
                {
                    Name = Name,
                    Departure = Departure,
                    Destination = Destination,
                    DepartureTime = DepartureTime,
                    ArrivalTime = DepartureTime.AddHours(1),
                    TotalSeat = TotalSeat,
                    AvailableSeat = TotalSeat,
                    Price = Price,

                };
                _context.BusInfo.Add(busInfo);
                _context.SaveChanges();
                var BusInfos = _context.BusInfo
                    .Where(x => x.Id == busInfo.Id).ToList();
                return BusInfos;
            }
            //If the user is not admin don't add the new line
            else
            {
                var BusInfos = _context.BusInfo
                    .Where(x => x.Name == Name && x.DepartureTime == DepartureTime).ToList();
                return BusInfos;
            }
        }

        [HttpDelete("delete-line")]
        public IEnumerable<BusInfo> DeleteLine(string Username, string Name, DateTime DepartureTime)
        {
            //Get User with username == Username
            var user = _context.User
                .Where(x => x.Username == Username).FirstOrDefault();

            //If the user is admin delete the line
            if (user.IsAdmin)
            {
                var busInfo = _context.BusInfo
                    .Where(x => x.Name == Name && x.DepartureTime == DepartureTime).FirstOrDefault();
                _context.BusInfo.Remove(busInfo);
                _context.SaveChanges();
                var busInfos = _context.BusInfo
                    .Where(x => x.Id == busInfo.Id).ToList();
                return busInfos;
            }
            //If the user is not admin don't delete the line
            else
            {
                var BusInfos = _context.BusInfo
                    .Where(x => x.Name == Name && x.DepartureTime == DepartureTime).ToList();
                return BusInfos;
            }
        }
    }
}
