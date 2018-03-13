using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxFire
{
    public class ObjectAddedEventArgs<T>
    {
        public T Object { get; private set; }

        public ObjectAddedEventArgs(T obj)
        {
            Object = obj;
        }
    }

    public class DatabaseHelper
    {
        public delegate void ObjectAddedEventHandler(object sender, ObjectAddedEventArgs<Album> e);
        public event ObjectAddedEventHandler ObjectAdded;
        public void OnObjectAdded(Album a)
        {
            ObjectAdded?.Invoke(this, new ObjectAddedEventArgs<Album>(a));
        }
        public string Path { get; }

        public DatabaseHelper(string path)
        {
            Path = path;
        }

        public void SetupDatabase()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path.Substring(0, Path.LastIndexOf('/')));
            var conn = new SQLiteConnection(Path);
            conn.CreateTable<Album>();
            conn.CreateTable<Track>();
        }
        public void AddAlbum(Album a)
        {
            var conn = new SQLiteConnection(Path);
            conn.Insert(a);
            OnObjectAdded(a);
        }
        public void AddTrack(Track t)
        {
            var conn = new SQLiteConnection(Path);
            conn.Insert(t);
        }
        public async Task<List<Track>> QueryTracks()
        {
            var conn = new SQLiteAsyncConnection(Path);
            return await conn.QueryAsync<Track>("select * from Track");
        }
        public async Task<List<Album>> QueryAlbums()
        {
            var conn = new SQLiteAsyncConnection(Path);
            return await conn.QueryAsync<Album>("select * from Album order by name");
        }
        public async Task<List<Album>> QueryAlbums(string albumName)
        {
            var conn = new SQLiteAsyncConnection(Path);
            return await conn.QueryAsync<Album>("select * from Album where name = ?", albumName);
        }
        /*public void AddContent(MultiValueDictionary<Album, string> dict)
        {
            foreach(var kvp in dict)
            {
                AddAlbum(kvp.Key);
                var list = kvp.Value;
                foreach(var path in list)
                {
                    Track t = new Track(path);
                    t.AlbumId = kvp.Key.Id;
                    AddTrack(t);
                }
            }
        }*/
        public async Task<List<Track>> QueryTracksFromAlbum(int id)
        {
            var conn = new SQLiteAsyncConnection(Path);
            return await conn.QueryAsync<Track>("select * from Track where AlbumId = ?", id);
        }
    }
}
