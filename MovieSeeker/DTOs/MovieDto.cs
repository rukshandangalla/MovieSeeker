using System.Collections.Generic;

namespace MovieSeeker.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }

        public string Poster { get; set; }

        public List<TheaterDto> Theaters { get; set; }

        public string Cast { get; set; }

        public string Trailer { get; set; }
    }
}