namespace MovieSeeker.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }

        public string Poster { get; set; }

        public TheaterDto Theater { get; set; }

        public string Cast { get; set; }

        public string Trailer { get; set; }
    }
}