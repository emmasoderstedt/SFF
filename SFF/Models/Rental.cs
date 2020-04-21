using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SFF.Models
{
    public class Rental
    {
        public long Id { get; set; }
        public long MovieId { get; set; }
        public long FilmclubId { get; set; }
        public bool IsLent { get; set; } = true;
        public DateTime Date { get; set; } = DateTime.Now;

        public Review Review { get; set; }

    }
}

