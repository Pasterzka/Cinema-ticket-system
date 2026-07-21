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
    public class MoviesController : ControllerBase
    {
        private readonly CineQueueDbContext _dbContext;

        public MoviesController(CineQueueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/movies -> Retrieves a list of all movies
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _dbContext.Movies.ToListAsync();
            return Ok(movies);
        }

        // GET: api/movies/{id}/showtimes -> Retrieves a list of showtimes for a specific movie
        [HttpGet("{id}/showtimes")]
        public async Task<IActionResult> GetShowTimes(Guid id)
        {
            var showTimes = await _dbContext.ShowTimes
                .Where(st => st.MovieId == id)
                .Include(st => st.Hall) // Include the Hall details
                .OrderBy(st => st.StartTime) // Order by start time
                .ToListAsync();
                
            return Ok(showTimes);
        }
    }
}