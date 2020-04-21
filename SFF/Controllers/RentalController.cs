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
    [Route("api/rentals")]

    public class RentalController : ControllerBase
    {
        private readonly SFFContext _context;

        public RentalController(SFFContext context)
        {
            _context = context;
        }

        //GET: Få alla rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRental()
        {
            
            return await _context.Rentals
                                        .Include(r => r.Review)
                                        .ToListAsync();
        }

        //GET: Få en rental med id
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(long id)
        {
            var rental = await _context.Rentals.FindAsync(id);

            if (rental == null)
            {
                return NotFound();
            }

            return rental;
        }


        //POST: Lägg till uthyrning
        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            var movie = _context.Movies.Include(m => m.Rentals).Where(m => m.Id == rental.MovieId).FirstOrDefault();

            var activeRentals = movie.Rentals.Where(r => r.IsLent == true);

            if (activeRentals.Count() < movie.MaximumRentals)
            {
                _context.Rentals.Add(rental);

                await _context.SaveChangesAsync();
                return rental;

            }
            else
            {
                return BadRequest();
            }
        }


        //sätt film som återlämnad
        [HttpPut ("return/{id}")]
        public async Task<ActionResult<Rental>> PutRental(long id)
        {
            var rental = _context.Rentals.Find(id);

            if (rental != null)
            {
                rental.IsLent = false;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if (!RentalExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return rental;

            }

            return NotFound();


        }

        //GET XML-data om specifik utlåning
        [HttpGet("label/{id}")]
        [Produces("application/xml")]
        public async Task<ActionResult<Label>> GetLabel(int id)
        {
            var rental = await _context.Rentals.Where(r => r.Id == id).FirstOrDefaultAsync();
            if (rental == null)
            {
                return NotFound();
            }

            
            var movie = await _context.Movies.Where(m => m.Id == rental.MovieId).FirstOrDefaultAsync();
            var filmclub = await _context.Filmclubs.Where(f => f.Id == rental.FilmclubId).FirstOrDefaultAsync();

            return new Label { Title = movie.Title, Location = filmclub.Location, Date = rental.Date };

        }

        private bool RentalExists(long id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }

    }
}