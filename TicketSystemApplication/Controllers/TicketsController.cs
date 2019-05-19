using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystemApplication.Data;
using TicketSystemApplication.Models;

namespace TicketSystemApplication.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase {
        private readonly TicketSystemApplicationContext _context;

        public TicketsController(TicketSystemApplicationContext context) {
            _context = context;
            _context.Database.EnsureCreated();

            if (!_context.Ticket.Any()) {
                _context.Ticket.Add(new Ticket {
                    RouteNo = "12345",
                    DepartureTime = DateTime.Today,
                    DepartureStopNo = "13",
                    DestinationStopNo = "25",
                    SeatNo = "40",
                    Price = 1m
                });
            }

            _context.SaveChanges();
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketList(string routeNo,
            string departureStopNo, string destinationStopNo) {
            return await _context.Ticket.Where(x => (routeNo == null || x.RouteNo == routeNo) &&
                                                    (departureStopNo == null || x.DepartureStopNo == departureStopNo) &&
                                                    (destinationStopNo == null ||
                                                     x.DestinationStopNo == destinationStopNo)
            ).ToListAsync();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(Guid id) {
            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null) {
                return NotFound();
            }

            return ticket;
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<ActionResult<Ticket>> AddTicket(Ticket ticket) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (ticket.DepartureStopNo == ticket.DestinationStopNo || ticket.DepartureTime <= DateTime.Now ||
                ticket.Price <= 0) {
                return BadRequest(ticket);
            }

            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new {id = ticket.Id}, ticket);
        }
    }
}
