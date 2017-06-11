using System;

namespace MovieSeeker.Data.Models
{
    [Serializable]
    public class Theater
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string Map { get; set; }

        public string Cordinates { get; set; }
    }
}