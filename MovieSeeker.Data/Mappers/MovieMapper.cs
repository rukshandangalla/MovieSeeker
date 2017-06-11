using MovieSeeker.Data.DTOs;
using MovieSeeker.Data.Models;
using System.Collections.Generic;

namespace MovieSeeker.Data.Mappers
{
    public static class MovieMapper
    {
        public static Movie Convert(MovieDto from)
        {
            var convertedTheaters = new List<Theater>();

            if (from.Theaters != null)
            {
                foreach (var theater in from.Theaters)
                {
                    convertedTheaters.Add(TheaterMapper.Convert(theater));
                }
            }

            return new Movie()
            {
                Id = from.Id,
                Name = from.Name,
                Poster = from.Poster,
                Genre = from.Genre,
                Theaters = convertedTheaters,
                Cast = from.Cast,
                Trailer = from.Trailer
            };
        }
    }
}
