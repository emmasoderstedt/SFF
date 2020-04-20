using System;
using System.ComponentModel.DataAnnotations;

namespace SFF.Models
{
    public class Rental
    {
        public long Id { get; set; }
        public long MovieId { get; set; }
        public long FilmclubId { get; set;}
        public bool IsLent { get; set; } = true;
        public DateTime Date { get; set; } = DateTime.Now;

        public string Trivia { get; set; }
        [Range (1,5)]
        public int Rating { get; set; } //begränsa så de bara går 1-5?
    }
}

