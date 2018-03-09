using SQLite;
using System.Collections.Generic;

namespace FoxFire
{
    public class Album
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        //public int? TrackCount { get { return Tracks?.Count; } }
        //public IList<Track> Tracks { get; set; }
        //public IList<Artist> Artists { get; set; }

        public Album()
        {
            //Tracks = new List<Track>();
            //Artists = new List<Artist>();
        }

        public override bool Equals(object obj)
        {
            return (obj as Album)?.Name == Name;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
