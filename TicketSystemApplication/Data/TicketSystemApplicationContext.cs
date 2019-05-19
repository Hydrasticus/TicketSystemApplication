using Microsoft.EntityFrameworkCore;
using TicketSystemApplication.Models;

namespace TicketSystemApplication.Data {
    public class TicketSystemApplicationContext : DbContext {
        public TicketSystemApplicationContext(DbContextOptions<TicketSystemApplicationContext> options)
            : base(options) { }

        public DbSet<Ticket> Ticket { get; set; }
    }
}
