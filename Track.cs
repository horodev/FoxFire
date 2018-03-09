using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FoxFire
{
    public class Track
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Path { get; private set; }
        /// <summary>
        /// Possible values are:
        ///     - audio/mp3
        ///     - audio/aac
        ///     - audio/wma
        ///     - audio/mp4
        ///     - audio/wav
        ///     - audio/ogg
        /// </summary>
        public string ContentType { get; set; }
        public int? TrackNumber { get; set; }
        public TimeSpan Duration { get; set; }
        public IList<Artist> Artists { get; set; }

        public Track()
        {
            Artists = new List<Artist>();
        }
        public Track(string path)
        {
            path = Path;
            Artists = new List<Artist>();
            /*
                var f = TagLib.File.Create(Path);

                Name = f.Tag.Title == null ?
                    path.Substring(path.LastIndexOf('/')) :
                    f.Tag.Title ;
                ContentType = null;
                TrackNumber = (int)f.Tag.TrackCount;
                Duration = new TimeSpan(f.Length);
                foreach (string a in f.Tag.AlbumArtists)
                    Artists.Add(new Artist(a));
                f.Dispose(); 
            */
        }
    }
}
