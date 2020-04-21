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
                return StatusCode(400, BadRequest());
            }
        }


        //sätt film som återlämnad
        [HttpPut ("return/{id}")]
        public async Task<ActionResult<Rental>> PutRental(long id)
        {
            var rental = _context.Rentals.Find(id);
            rental.IsLent = false;

            await _context.SaveChangesAsync();
            return rental;
        }

        //GET XML-data om specifik film
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

    }
}