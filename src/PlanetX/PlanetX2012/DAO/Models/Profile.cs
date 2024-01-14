using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Profile
    {
        public long ProfileId { get; set; }
        public int UserId { get; set; }
        public sbyte Privacy { get; set; }
        public Nullable<sbyte> Rating { get; set; }
        public Nullable<System.DateTime> Dob { get; set; }
        public string AboutMe { get; set; }
        public string Relationship { get; set; }
        public string LookingFor { get; set; }
        public string Phone { get; set; }
        public string Interests { get; set; }
        public string Education { get; set; }
        public string Hobbies { get; set; }
        public string FavMovies { get; set; }
        public string FavArtists { get; set; }
        public string FavBooks { get; set; }
        public string FavAnimals { get; set; }
        public Nullable<sbyte> Religion { get; set; }
        public string EverythingElse { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
