using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System;
using System.Linq;
using Travelephant.Data;
using Travelephant.Model;
using Travelephant.Show;

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
        public IEnumerable<TicketToShow> Get()
        {
            var Tickets = _context.Ticket.ToList();

            var busInfo = _context.BusInfo.ToList();

            var userInfo = _context.User.ToList();

            var TicketsToShow = Tickets.Select(x => new TicketToShow
            {
                Name = x.User.Name,
                Surname = x.User.Surname,
                Departure = x.Bus.Departure,
                Destination = x.Bus.Destination,
                Active = x.IsActive ? "Aktive" : " Jo Aktive"
            });

            return TicketsToShow;
        }

        [HttpGet("get-tickets")]
        public IEnumerable<TicketToShow> GetTickets(int Id)
        {
            var Tickets = _context.Ticket
                .Where(x => x.UserID == Id).ToList();

            var busInfo = _context.BusInfo.ToList();

            var userInfo = _context.User
                .Where(x => x.UserId == Id).FirstOrDefault();

            var TicketsToShow = Tickets.Select(x => new TicketToShow
            {
                Name = x.User.Name,
                Surname = x.User.Surname,
                Departure = x.Bus.Departure,
                Destination = x.Bus.Destination,
                Active = x.IsActive ? "Aktive" : " Jo Aktive"
            });

            return TicketsToShow;
        }

        [HttpPut("cancel-ticket")]
        public IEnumerable<TicketToShow> CancelTicket(int UserID, int BusID)
        {
            //Get ticket with that UserID and BusID that if it is reserved
            var ticket = _context.Ticket
                .Where(x => x.UserID == UserID && x.BusID == BusID && x.IsActive).FirstOrDefault();
            //Get busline infos with BusID
            var busInfo = _context.BusInfo
                .Where(x => x.BusId == BusID).FirstOrDefault();

            var userInfo = _context.User
                .Where(x => x.UserId == UserID).FirstOrDefault();

            if (ticket != null)
            {
                ticket.IsActive = false;
                busInfo.AvailableSeat++;
            }

            _context.SaveChanges();
            var Tickets = _context.Ticket
                .Where(x => x.UserID == UserID && x.BusID == BusID && !x.IsActive).ToList();

            var TicketsToShow = Tickets.Select(x => new TicketToShow
            {
                Name = x.User.Name,
                Surname = x.User.Surname,
                Departure = x.Bus.Departure,
                Destination = x.Bus.Destination,
                Active = x.IsActive ? "Aktive" : " Jo Aktive"
            });

            return TicketsToShow;
        }

        [HttpPost("book-ticket")]
        public IEnumerable<TicketToShow> BookTicket(int UserID, int BusID)
        {
            //Get busline infos with BusID
            var busInfo = _context.BusInfo
                .Where(x => x.BusId == BusID).FirstOrDefault();

            var userInfo = _context.User
                .Where(x => x.UserId == UserID).FirstOrDefault();

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

            var TicketsToShow = Tickets.Select(x => new TicketToShow
            {
                Name = x.User.Name,
                Surname = x.User.Surname,
                Departure = x.Bus.Departure,
                Destination = x.Bus.Destination,
                Active = x.IsActive ? "Aktive" : " Jo Aktive"
            });

            return TicketsToShow;
        }
    }
}