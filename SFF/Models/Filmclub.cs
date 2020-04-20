using System;
namespace SFF.Models
{
    public class Filmclub
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
