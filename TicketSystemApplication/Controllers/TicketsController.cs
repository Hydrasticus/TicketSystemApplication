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
        /// <summary>
        /// Returns an array of tickets, optionally filtered by route number 
        /// and/or departure stop number and/or destination stop number.
        /// </summary>
        /// <param name="routeNo">Optional parameter.</param>
        /// <param name="departureStopNo">Optional parameter.</param>
        /// <param name="destinationStopNo">Optional parameter.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketList(string routeNo,
            string departureStopNo, string destinationStopNo) {
            return await _context.Ticket.Where(
                x => (routeNo == null || x.RouteNo == routeNo) &&
                     (departureStopNo == null || x.DepartureStopNo == departureStopNo) &&
                     (destinationStopNo == null || x.DestinationStopNo == destinationStopNo)
            ).ToListAsync();
        }

        // GET: api/Tickets/5
        /// <summary>
        /// Returns a ticket via its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(Guid id) {
            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null) {
                return NotFound();
            }

            return Ok(ticket);
        }

        // POST: api/Tickets
        /// <summary>
        /// Adds a ticket to the database.
        /// </summary>
        /// <param name="ticket">Ticket from JSON body.</param>
        /// <returns>Created ticket.</returns>
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

        // PUT: api/Tickets/5
        /// <summary>
        /// Updates a ticket via its ID. First checks whether it exists.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(Guid id, Ticket ticket) {
            if (id != ticket.Id) {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!TicketExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Tickets/5
        /// <summary>
        /// Deletes a ticket via its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ticket>> DeleteTicket(Guid id) {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null) {
                return NotFound();
            }

            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        private bool TicketExists(Guid id) {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
