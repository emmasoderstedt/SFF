using System;
using System.ComponentModel.DataAnnotations;

namespace SFF.Models
{
    public class Review
    {
        
        public long Id { get; set; }
        public string Trivia { get; set; }
        [Range (1, 5)]
        public int Rating { get; set; }

        public long RentalId { get; set; }

    }
}
