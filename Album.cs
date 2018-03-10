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

        public Album()
        {
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
