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
            var newreview = new Review { Trivia = review.Trivia, Rating = review.Rating, RentalId = id };
            _context.Reviews.Add(newreview);
            await _context.SaveChangesAsync();

            return newreview;
        }

        //Ta bort trivia
        [HttpPut("removetrivia/{id}")]
        public async Task<ActionResult<Review>> DeleteTrivia(long id)
        {
            var review = await _context.Reviews.Where(r => r.Id == id).FirstOrDefaultAsync();
            review.Trivia = null;
            await _context.SaveChangesAsync();

            return review;
        }
    }
}
