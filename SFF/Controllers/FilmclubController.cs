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
    [Route("api/filmclubs")]

    public class FilmclubController : ControllerBase
    {
        private readonly SFFContext _context;

        public FilmclubController(SFFContext context)
        {
            _context = context;
        }

        //GET: Få alla filmclubs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filmclub>>> GetFilmclub()
        {

            return await _context.Filmclubs.ToListAsync();
        }

        //GET: Få en filmklubb 
        [HttpGet ("{id}")]
        public async Task<ActionResult<Filmclub>> GetFilmclub(long id)
        {
           
            var filmclub = await _context.Filmclubs.FindAsync(id);

            if (filmclub == null)
            {
                return NotFound();
            }

            return filmclub;

        }

        //POST: Lägg till filmclub
        [HttpPost]
        public async Task<ActionResult<Filmclub>> PostFilmClub(Filmclub filmclub)
        {
            _context.Filmclubs.Add(filmclub);
            await _context.SaveChangesAsync();
            return CreatedAtAction("PostFilmclub", new { id = filmclub.Id }, filmclub);
        }

        //ta bort filmklubb
        [HttpDelete("{id}")]
        public async Task<ActionResult<Filmclub>> DeleteFilmclub(long id)
        {
            var filmclub = _context.Filmclubs.Find(id);

            if (filmclub != null)
            {
                _context.Filmclubs.Remove(filmclub);
                await _context.SaveChangesAsync();
                return filmclub;

            }
            return NotFound();
        }


        //ändra namn på filmklubb
        [HttpPut ("changename/{id}")]
        public async Task<ActionResult<Filmclub>> PutFilmclubName(long id, Filmclub filmclub)
        {
            var oldFilmclub = _context.Filmclubs.Find(id);

            if (oldFilmclub != null)
            {
                oldFilmclub.Name = filmclub.Name;

                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmclubExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                return oldFilmclub;

            }
            return NotFound();

        }

        //ändra ort på filmklubb
        [HttpPut("changelocation/{id}")]
        public async Task<ActionResult<Filmclub>> PutFilmclubLocation(long id, Filmclub filmclub)
        {
            var oldFilmclub = _context.Filmclubs.Find(id);

            if (oldFilmclub != null)
            {
                oldFilmclub.Location = filmclub.Location;

                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmclubExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return oldFilmclub;

            }
            return NotFound();

        }

        //Get alla filmer lånade av filmstudion
        [HttpGet("{id}/movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetRentedMovies(long id)
        {
            var filmclub = await _context.Filmclubs.FindAsync(id);

            if (filmclub == null)
            {
                return NotFound();
            }

            var rentals =  _context.Rentals
                                        .Where(r => r.IsLent == true && r.FilmclubId == filmclub.Id);

            var movies = new List<Movie>();

            foreach (var rental in rentals)
            {
                var movie = _context.Movies.Where(m => m.Id == rental.MovieId);
                movies.AddRange(movie);
            }

            return movies;
        }

        private bool FilmclubExists(long id)
        {
            return _context.Filmclubs.Any(e => e.Id == id);
        }

    }
}