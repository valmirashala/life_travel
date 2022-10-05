using Microsoft.AspNetCore.Mvc;
using Travelephant.Data;
using Travelephant.Model;

namespace Travelephant.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TravelephantContext _context;

        public TicketsController(TravelephantContext context)
        {
            _context = context;
        }
        [HttpGet("get-all-tickets")]
        public IEnumerable<Ticket> Get()
        {
            var Tickets = _context.Ticket.ToList();
            return Tickets;
        }

        [HttpGet("get-tickets")]
        public IEnumerable<Ticket> GetTickets(int Id)
        {
            var Tickets = _context.Ticket
                .Where(x => x.UserID == Id && x.IsActive).ToList();
            
            return Tickets;
        }

        [HttpPut("cancel-ticket")]
        public IEnumerable<Ticket> CancelTicket(int UserID, int BusID)
        {
            //Get ticket with that UserID and BusID that if it is reserved
            var ticket = _context.Ticket
                .Where(x => x.UserID == UserID && x.BusID == BusID && x.IsActive).FirstOrDefault();
            //Get busline infos with BusID
            var busInfo = _context.BusInfo
                .Where(x => x.BusId == BusID).FirstOrDefault();

            if (ticket != null)
            {
                ticket.IsActive = false;
                busInfo.AvailableSeat++;
            }

            _context.SaveChanges();
            var Tickets = _context.Ticket
                .Where(x => x.UserID == UserID && x.BusID == BusID).ToList();
            return Tickets;
        }

        [HttpPost("book-ticket")]
        public IEnumerable<Ticket> BookTicket(int UserID, int BusID)
        {
            //Get busline infos with BusID
            var busInfo = _context.BusInfo
                .Where(x => x.BusId == BusID).FirstOrDefault();

            if (busInfo.AvailableSeat > 0)
            {
                busInfo.AvailableSeat--;
            }
            var ticket = new Ticket
            {
                UserID = UserID,
                BusID = BusID,
                IsActive = true,
            };

            _context.Ticket.Add(ticket);
            _context.SaveChanges();
            var Tickets = _context.Ticket
                .Where(x => x.UserID == UserID && x.BusID == BusID).ToList();

            return Tickets;
        }
    }
}
