using SQLite;
using System.Collections.Generic;

namespace FoxFire
{
    public class Artist
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public IList<Album> Albums { get; set; }

        public Artist()
        {
            Albums = new List<Album>();
        }
        public Artist(string a)
        {
            Albums = new List<Album>();
            Name = a;
        }
    }
}