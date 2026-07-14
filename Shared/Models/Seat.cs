using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Seat
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public SeatStatus Status { get; set; }

        // Foreign key to the Movie entity
        public Guid MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}