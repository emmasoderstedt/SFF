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
    [Route("api/filmclub")] //filmclubs!

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
            _context.Filmclubs.Remove(filmclub);
            await _context.SaveChangesAsync();
            return filmclub;
        }


        //ändra namn på filmklubb
        [HttpPut ("changename/{id}")]
        public async Task<ActionResult<Filmclub>> PutFilmclubName(long id, Filmclub filmclub)
        {
            var oldFilmclub = _context.Filmclubs.Find(id);
            oldFilmclub.Name = filmclub.Name;
            await _context.SaveChangesAsync();
            return oldFilmclub;

        }

        //ändra ort på filmklubb
        [HttpPut("changelocation/{id}")]
        public async Task<ActionResult<Filmclub>> PutFilmclubLocation(long id, Filmclub filmclub)
        {
            var oldFilmclub = _context.Filmclubs.Find(id);
            oldFilmclub.Location = filmclub.Location;
            await _context.SaveChangesAsync();
            return oldFilmclub;

        }

        //Get alla filmer lånade av filmstudion
        [HttpGet("{id}/movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetRentedMovies(long id)
        {
            var filmclub = await _context.Filmclubs.FindAsync(id);

            //vilka rentals som är aktiva och tillhör filmklubbens
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

    }
}