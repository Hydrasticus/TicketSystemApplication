using System;
using System.ComponentModel.DataAnnotations;

namespace TicketSystemApplication.Models {
    public class Ticket {

        [Key]
        public Guid Id { get; private set; }

        [Required]
        public string RouteNo { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public string DepartureStopNo { get; set; }

        [Required]
        public string DestinationStopNo { get; set; }

        [Required]
        public string SeatNo { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
