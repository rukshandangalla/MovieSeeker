using System;

namespace MovieSeeker.Models
{
    [Serializable]
    public class Theater
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }
    }
}