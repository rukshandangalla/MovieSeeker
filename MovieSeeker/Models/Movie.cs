using System;
using System.Collections.Generic;

namespace MovieSeeker.Models
{
    [Serializable]
    public class Movie
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }

        public string Poster { get; set; }

        public List<Theater> Theaters { get; set; }

        public string Cast { get; set; }

        public string Trailer { get; set; }
    }
}