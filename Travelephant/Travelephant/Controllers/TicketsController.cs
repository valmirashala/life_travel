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
                ID = x.ID,
                Name = x.User.Name,
                Surname = x.User.Surname,
                Departure = x.Bus.Departure,
                Destination = x.Bus.Destination,
                Active = x.IsActive ? "Aktive" : " Jo Aktive"
            });

            return TicketsToShow;
        }

        [HttpGet("get-tickets")]
        public IEnumerable<TicketToShow> GetTickets(string username)
        {
            if(string.IsNullOrWhiteSpace(username))
                return Enumerable.Empty<TicketToShow>();

            var userInfo = _context.User
                .Where(x => x.Username.ToLower() == username.ToLower()).FirstOrDefault();

            if (userInfo == null)
                return Enumerable.Empty<TicketToShow>();

            var Tickets = _context.Ticket
                .Where(x => x.UserID == userInfo.UserId).ToList();

            var busInfo = _context.BusInfo.ToList();

            var TicketsToShow = Tickets.Select(x => new TicketToShow
            {
                ID = x.ID,
                Name = x.User.Name,
                Surname = x.User.Surname,
                Departure = x.Bus.Departure,
                Destination = x.Bus.Destination,
                Active = x.IsActive ? "Aktive" : " Jo Aktive"
            });

            return TicketsToShow;
        }

        [HttpPut("cancel-ticket")]
        public IEnumerable<TicketToShow> CancelTicket(int TickedId)
        {
            //Get ticket with that UserID and BusID that if it is reserved
            var ticket = _context.Ticket
                .Where(x => x.ID == TickedId && x.IsActive).FirstOrDefault();

            //Get busline infos with BusID
            var busInfo = _context.BusInfo
                .Where(x => x.BusId == ticket.BusID).FirstOrDefault();

            var userInfo = _context.User
                .Where(x => x.UserId == ticket.UserID).FirstOrDefault();

            if (ticket != null)
            {
                ticket.IsActive = false;
                busInfo.AvailableSeat++;
            }
            _context.SaveChanges();
            var Tickets = _context.Ticket
                .Where(x => x.ID == TickedId && x.IsActive).ToList();

            var TicketsToShow = Tickets.Select(x => new TicketToShow
            {
                ID = x.ID,
                Name = x.User.Name,
                Surname = x.User.Surname,
                Departure = x.Bus.Departure,
                Destination = x.Bus.Destination,
                Active = x.IsActive ? "Aktive" : " Jo Aktive"
            });

            return TicketsToShow;
        }

        [HttpPost("book-ticket")]
        public IEnumerable<TicketToShow> BookTicket(int UserID, string Departure, int DepartureTime)
        {
            //Get busline infos with BusID
            var busInfo = _context.BusInfo
                .Where(x => x.Departure == Departure && x.DepartureTime == DepartureTime).FirstOrDefault();

            var userInfo = _context.User
                .Where(x => x.UserId == UserID).FirstOrDefault();

            if (busInfo.AvailableSeat > 0)
            {
                busInfo.AvailableSeat--;
            }
            var ticket = new Ticket
            {
                UserID = UserID,
                BusID = busInfo.BusId,
                IsActive = true,
            };

            _context.Ticket.Add(ticket);
            _context.SaveChanges();
            var Tickets = _context.Ticket
                .Where(x => x.UserID == UserID && x.BusID == ticket.BusID).ToList();

            var TicketsToShow = Tickets.Select(x => new TicketToShow
            {
                ID = x.ID,
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