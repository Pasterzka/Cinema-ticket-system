using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid SeatId { get; set; }
        public Seat? Seat { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime ReservationTime { get; set; } = DateTime.UtcNow;
    }
}