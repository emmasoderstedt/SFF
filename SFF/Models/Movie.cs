using System;
using System.Collections.Generic;

namespace SFF.Models
{
    public class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int MaximumRentals { get; set; }
        public bool IsDigital { get; set; }

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    }
}
