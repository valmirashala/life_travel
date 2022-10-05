using Microsoft.AspNetCore.Mvc;
using Travelephant.Body;
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
            var AllBusInfo = _context.BusInfo.ToList();
            return AllBusInfo;
        }

        [HttpGet("sepcific-bus-info")]
        public IEnumerable<BusInfo> Get(string Departure, string Destination,
            int? fromTime, int? toTime)
        {
            //Filters lines that satart at Departure and ends on Destination
            //Direct lines from Departure to Destination
            var FilteredData = _context.BusInfo
                .Where(x => x.Departure == Departure && x.Destination == Destination
                && x.IsActive && (fromTime == null || x.DepartureTime >= fromTime)
                && (toTime == null || x.ArrivalTime <= toTime)).ToList();

            //If there is no direct line
            if (FilteredData.Count == 0)
            {
                //Filter all lines that starts from StartCiti
                var FirstFilter = _context.BusInfo
                    .Where(x => x.Departure == Departure && x.IsActive
                    && (fromTime == null || x.DepartureTime >= fromTime)
                    && (toTime == null || x.ArrivalTime <= toTime)).ToList();

                //Filter all lines that ends on Destination
                var SecondFilter = _context.BusInfo
                    .Where(x => x.Destination == Destination && x.IsActive
                    && (fromTime == null || x.DepartureTime >= fromTime)
                    && (toTime == null || x.ArrivalTime <= toTime)).ToList();

                var FirstFiltered = new List<BusInfo>();
                var SecondFiltered = new List<BusInfo>();

                foreach (var item in FirstFilter)
                {
                    //Takes only lines that ends on Destination and start from the Destination
                    //of lines that start from Departure
                    var tmp = SecondFilter.Where(x => x.Departure == item.Destination
                        && x.IsActive).ToList();
                    if (tmp.Count != 0)
                    {
                        FirstFiltered = new List<BusInfo>(tmp);
                    }
                }

                foreach (var item in SecondFilter)
                {
                    //Takes only lines that starts on Departure and end on Departure
                    //of lines that ends on Destination
                    var tmp = FirstFilter.Where(x => x.Destination == item.Departure
                        && x.IsActive).ToList();
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
        public int GetBusLineID(string Name, int DepartureTime)
        {
            var busInfo = _context.BusInfo
                .Where(x => x.Name == Name && x.DepartureTime == DepartureTime
                    && x.IsActive).FirstOrDefault();
            return busInfo.BusId;
        }

        [HttpPut("set-price")]
        public IEnumerable<BusInfo> SetPrice(string Username, string Name,
            int DepartureTime, double Price)
        {
            //Get User with username == Username
            var user = _context.User
                .Where(x => x.Username == Username).FirstOrDefault();

            if (user.IsAdmin)
            {
                var busInfo = _context.BusInfo
                    .Where(x => x.Name == Name && x.DepartureTime == DepartureTime
                        && x.IsActive).FirstOrDefault();

                busInfo.Price = Price;

                _context.SaveChanges();
                var BusInfos = _context.BusInfo
                    .Where(x => x.BusId == busInfo.BusId).ToList();
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
        public IEnumerable<BusInfo> AddBusLine(string Username, BusInfoBody busInfoBody)
        {
            //Get User with username == Username
            var user = _context.User
                .Where(x => x.Username == Username).FirstOrDefault();

            //If the user is admin add the new line
            if (user.IsAdmin)
            {
                var busLine = _context.BusInfo
                    .Where(x => x.Departure == busInfoBody.Departure
                    && x.DepartureTime == busInfoBody.DepartureTime
                    && !x.IsActive).FirstOrDefault();

                if (busLine != null)
                {
                    busLine.IsActive = true;
                    _context.SaveChanges();
                    var busLines = _context.BusInfo
                    .Where(x => x.BusId == busLine.BusId).ToList();
                    return busLines;
                }

                var busInfo = new BusInfo
                {
                    Name = busInfoBody.Name,
                    Departure = busInfoBody.Departure,
                    Destination = busInfoBody.Destination,
                    DepartureTime = busInfoBody.DepartureTime,
                    ArrivalTime = busInfoBody.DepartureTime + 100,
                    TotalSeat = busInfoBody.TotalSeat,
                    AvailableSeat = busInfoBody.TotalSeat,
                    Price = busInfoBody.Price,
                    IsActive = true

                };
                _context.BusInfo.Add(busInfo);
                _context.SaveChanges();
                var BusInfos = _context.BusInfo
                    .Where(x => x.BusId == busInfo.BusId).ToList();
                return BusInfos;
            }
            //If the user is not admin don't add the new line
            else
            {
                return Enumerable.Empty<BusInfo>();
            }
        }

        [HttpDelete("delete-line")]
        public IEnumerable<BusInfo> DeleteLine(string Username, string Name, int DepartureTime)
        {
            //Get User with username == Username
            var user = _context.User
                .Where(x => x.Username == Username).FirstOrDefault();

            //If the user is admin delete the line
            if (user.IsAdmin)
            {
                var busInfo = _context.BusInfo
                    .Where(x => x.Name == Name && x.DepartureTime == DepartureTime).FirstOrDefault();
                busInfo.IsActive = false;
                _context.BusInfo.Update(busInfo);
                _context.SaveChanges();
                var BusInfos = _context.BusInfo
                    .Where(x => x.BusId == busInfo.BusId).ToList();
                return BusInfos;
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
