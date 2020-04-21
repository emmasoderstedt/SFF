using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFF.Models;


namespace SFF.Controllers
{
    [ApiController]
    [Route("api/movies")]

    public class MoviesController : ControllerBase
    {
        private readonly SFFContext _context;

        public MoviesController(SFFContext context)
        {
            _context = context;
        }



        //GET: Få alla filmer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.Include(m => m.Rentals).ToListAsync();
        }

        //GET: Få en film med id
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovies(long id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }


        //POST: Lägg till film
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return CreatedAtAction("PostMovie", new { id = movie.Id }, movie);
        }



        //PUT: Ändra en filxms uthyrnings gräns
        [Route ("{id}/{value}")]
        [HttpPut]
        public async Task<ActionResult<Movie>> PutMovieMaximalRentals(long id, int value)
        {
            var movie = _context.Movies.Find(id);

            if (movie != null)
            {
                movie.MaximumRentals = value;

                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return movie;
            }

            return NotFound();
        }

        private bool MovieExists(long id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

    }
}
