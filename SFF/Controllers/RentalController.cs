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
    [Route("api/rentals")] //skriv ut 

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
            
            return await _context.Rentals.ToListAsync();
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
            rental.IsLent = false;

            await _context.SaveChangesAsync();
            return rental;
        }

        //Skapa ny trivia som håller text
        [HttpPut("trivia/{id}")]
        public async Task<ActionResult<Rental>> PutTrivia(long id , Rental rental)
        {
            var oldRental = _context.Rentals.Find(id);
            oldRental.Trivia = rental.Trivia;
            await _context.SaveChangesAsync();
            return rental;
        }

        //Ta bort trivia
        [HttpPut("removetrivia/{id}")]
        public async Task<ActionResult<Rental>> DeleteTrivia(long id)
        {
            var rental = _context.Rentals.Find(id);
            rental.Trivia = null;
            await _context.SaveChangesAsync();
            return rental;
        }


        //Lägg till ett betyg på filmen i denna rental
        [HttpPut("rating/{id}")]
        public async Task<ActionResult<Rental>> PutRating(long id ,Rental rental)
        {
            var oldRental = _context.Rentals.Find(id);
            oldRental.Rating = rental.Rating;

            await _context.SaveChangesAsync();
            return rental;
        }

        //GET XML-data om specifik film
        [HttpGet("lable/{id}")]
        [Produces("application/xml")]
        public async Task<ActionResult<Lable>> GetLable(int id)
        {
            var rental = await _context.Rentals.Where(r => r.Id == id).FirstOrDefaultAsync();
            if (rental == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.Where(m => m.Id == rental.MovieId).FirstOrDefaultAsync();
            var filmclub = await _context.Filmclubs.Where(f => f.Id == rental.FilmclubId).FirstOrDefaultAsync();



            return new Lable { Title = movie.Title, Location = filmclub.Location, Date = rental.Date };

        }

    }
}