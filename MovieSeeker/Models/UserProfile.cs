using System;

namespace MovieSeeker.Models
{
    [Serializable]
    public class UserProfile
    {
        public string Name { get; set; }

        public string Location { get; set; }
    }
}