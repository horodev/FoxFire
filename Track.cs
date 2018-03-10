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
        public int AlbumId { get; set; }
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

        public Track()
        {

        }
        public Track(string path)
        {
            Path = path;
            var f = TagLib.File.Create(Path);
            Name = f.Tag.Title == null ?
                path.Substring(path.LastIndexOf("//") + 1) :
                f.Tag.Title ;
            ContentType = null;
            TrackNumber = (int)f.Tag.TrackCount;
            Duration = new TimeSpan(f.Length);
            f.Dispose(); 
        }
    }
}
