using MovieSeeker.DTOs;
using MovieSeeker.Models;

namespace MovieSeeker.Mappers
{
    public static class TheaterMapper
    {
        public static Theater Convert(TheaterDto from)
        {
            return new Theater()
            {
                Id = from.Id,
                Name = from.Name,
                City = from.City
            };
        }
    }
}