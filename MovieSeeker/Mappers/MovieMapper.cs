using MovieSeeker.DTOs;
using MovieSeeker.Models;

namespace MovieSeeker.Mappers
{
    public static class MovieMapper
    {
        public static Movie Convert(MovieDto from)
        {
            return new Movie()
            {
                Id = from.Id,
                Name = from.Name,
                Poster = from.Poster,
                Genre = from.Genre,
                Theater = from.Theater != null ? TheaterMapper.Convert(from.Theater) : null,
                Cast = from.Cast,
                Trailer = from.Trailer
            };
        }
    }
}