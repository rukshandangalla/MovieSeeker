using MovieSeeker.Data.DTOs;
using MovieSeeker.Data.Models;

namespace MovieSeeker.Data.Mappers
{
    public static class TheaterMapper
    {
        public static Theater Convert(TheaterDto from)
        {
            return new Theater()
            {
                Id = from.Id,
                Name = from.Name,
                Location = from.Location,
                Map = from.Map,
                Cordinates = from.Cordinates
            };
        }
    }
}
