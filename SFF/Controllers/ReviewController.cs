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
    [Route("api/reviews")]

    public class ReviewController : ControllerBase
    {
        private readonly SFFContext _context;

        public ReviewController(SFFContext context)
        {
            _context = context;
        }


        //Skapa ny review med trivia och/eller betyg
        [HttpPost("{id}")]
        public async Task<ActionResult<Review>> PutTrivia(Review review, long id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental.Review == null)
            {
                var newreview = new Review { Trivia = review.Trivia, Rating = review.Rating, RentalId = id };
                _context.Reviews.Add(newreview);
                await _context.SaveChangesAsync();

                return newreview;

            }
            return BadRequest();
        }

        //Ta bort trivia
        [HttpPut("removetrivia/{id}")]
        public async Task<ActionResult<Review>> DeleteTrivia(long id)
        {
            var review = await _context.Reviews.Where(r => r.Id == id).FirstOrDefaultAsync();
            if (review != null)
            {
                review.Trivia = null;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return review;
            }

            return NotFound();
        }

        private bool ReviewExists(long id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
