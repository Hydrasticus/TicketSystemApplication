using System;
using System.ComponentModel.DataAnnotations;

namespace TicketSystemApplication.Models {
    public class Ticket {
        
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key] public Guid Id { get; private set; }

        /// <summary>
        /// Route number.
        /// </summary>
        [Required] public string RouteNo { get; set; }

        /// <summary>
        /// Departure date and time.
        /// </summary>
        [Required] public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Also known as departure stop number.
        /// </summary>
        [Required] public string DepartureStopNo { get; set; }

        /// <summary>
        /// Also known as destination stop number.
        /// </summary>
        [Required] public string DestinationStopNo { get; set; }

        /// <summary>
        /// Also known as seat number.
        /// </summary>
        [Required] public string SeatNo { get; set; }

        /// <summary>
        /// The price is rice.
        /// </summary>
        [Required] public decimal Price { get; set; }
    }
}
