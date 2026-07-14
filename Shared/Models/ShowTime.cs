using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class ShowTime
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }

        // Foreign key to the Movie entity
        public Guid MovieId { get; set; }
        public Movie? Movie { get; set; }

        // Foreign key to the Hall entity
        public Guid HallId { get; set; }
        public Hall? Hall { get; set; }
    }
}