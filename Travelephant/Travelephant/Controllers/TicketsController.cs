using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("getalltickets")]
        public IEnumerable<Ticket> Get()
        {
            var Tickets = _context.Ticket.ToList();
            return Tickets;
        }

        [HttpGet("gettickets")]
        public IEnumerable<Ticket> GetTickets(int Id)
        {
            var Tickets = _context.Ticket
                .Where(x => x.UserID == Id && x.IsActive).ToList();
            
            return Tickets;
        }

        [HttpPut("cancelticket")]
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

        [HttpPut("bookticket")]
        public IEnumerable<Ticket> BookTicket(int UserID, int BusID)
        {
            //Get ticket with that UserID and BusID that if it is not reserved
            var ticket = _context.Ticket
                .Where(x => x.UserID == UserID && x.BusID == BusID && !x.IsActive).FirstOrDefault();
            //Get busline infos with BusID
            var busInfo = _context.BusInfo
                .Where(x => x.BusId == BusID).FirstOrDefault();

            if (ticket != null)
            {
                ticket.IsActive = true;
                busInfo.AvailableSeat--;
            }

            var Tickets = _context.Ticket
                .Where(x => x.UserID == UserID && x.BusID == BusID).ToList();

            _context.SaveChanges();
            return Tickets;
        }
    }
}
