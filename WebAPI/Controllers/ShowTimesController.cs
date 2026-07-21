using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowTimesController : ControllerBase
    {
        private readonly CineQueueDbContext _dbContext;

        public ShowTimesController(CineQueueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/showtime{id}/seats -> Retrieves a list of seats for a specific showtime
        [HttpGet("{id}/seats")]
        public async Task<IActionResult> GetSeats(Guid id)
        {
            var seats = await _dbContext.Seats
                .Where(s => s.ShowTimeId == id)
                .OrderBy(s => s.Row) // Order by row
                .ThenBy(s => s.Number) // Then order by seat number
                .ToListAsync();

            if (seats == null || !seats.Any())
            {
                return NotFound(new { message = "No seats found for the specified showtime." });
            }

            return Ok(seats);
        }
    }
}