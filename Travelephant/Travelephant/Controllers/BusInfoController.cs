using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Travelephant.Body;
using Travelephant.Data;
using Travelephant.Model;
using Travelephant.Show;

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
        public List<BusInfoToShow> GetAll()
        {
            var AllBusInfo = _context.BusInfo.ToList();
            var AllBusInfoToSHow = AllBusInfo.Select(x => new BusInfoToShow
            {
                ID = x.BusId,
                Name = x.Name,
                Departure = x.Departure,
                DepartureTime = x.DepartureTime,
                Destination = x.Destination,
                ArrivalTime = x.ArrivalTime,
                Price = x.Price,
                AvailableSeats = x.AvailableSeat
            }).ToList();
            return AllBusInfoToSHow;
        }

        [HttpGet("sepcific-bus-info")]
        public IEnumerable<BusInfoToShow> Get(string Departure, string Destination,
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
                var SecondFilteredDataToShow = SecondFiltered.Select(x => new BusInfoToShow
                {
                    ID = x.BusId,
                    Name = x.Name,
                    Departure = x.Departure,
                    DepartureTime = x.DepartureTime,
                    Destination = x.Destination,
                    ArrivalTime = x.ArrivalTime,
                    Price = x.Price,
                    AvailableSeats = x.AvailableSeat
                });

                var FirstFilteredDataToShow = FirstFiltered.Select(x => new BusInfoToShow
                {
                    ID = x.BusId,
                    Name = x.Name,
                    Departure = x.Departure,
                    DepartureTime = x.DepartureTime,
                    Destination = x.Destination,
                    ArrivalTime = x.ArrivalTime,
                    Price = x.Price,
                    AvailableSeats = x.AvailableSeat
                });



                return SecondFilteredDataToShow.Union(FirstFilteredDataToShow);
            }


            var FilteredDataToShow = FilteredData.Select(x => new BusInfoToShow
            {
                ID = x.BusId,
                Name = x.Name,
                Departure = x.Departure,
                DepartureTime = x.DepartureTime,
                Destination = x.Destination,
                ArrivalTime = x.ArrivalTime,
                Price = x.Price,
                AvailableSeats = x.AvailableSeat
            });
            return FilteredDataToShow;
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
        public IEnumerable<BusInfo> AddBusLine(string Username, [FromBody] BusInfoBody busInfoBody)
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

        [HttpPut("update-bus-line")]
        public BusInfo UpdateBusLine(string Username, [FromBody] BusInfoBody busInfoBody)
        {
            //Get User with username == Username
            var user = _context.User
                .Where(x => x.Username == Username).FirstOrDefault();

            //If the user is admin update the bus line
            if (user.IsAdmin)
            {
                var busLineFromDb = _context.BusInfo
                    .Where(x => x.Departure == busInfoBody.Departure && x.DepartureTime == busInfoBody.DepartureTime).FirstOrDefault();

                if (busLineFromDb == null)
                    return new BusInfo();

                busLineFromDb.Name = busInfoBody.Name;
                busLineFromDb.Departure = busInfoBody.Departure;
                busLineFromDb.DepartureTime = busInfoBody.DepartureTime;
                busLineFromDb.Destination = busInfoBody.Destination;
                busLineFromDb.ArrivalTime = busInfoBody.DepartureTime + 100;
                busLineFromDb.TotalSeat = busInfoBody.TotalSeat;
                busLineFromDb.AvailableSeat = busInfoBody.TotalSeat;
                busLineFromDb.Price = busInfoBody.Price;

                _context.BusInfo.Update(busLineFromDb);
                _context.SaveChanges();

                return busLineFromDb;
            }
            else
            {
                return new BusInfo();
            }
        }
    }
}