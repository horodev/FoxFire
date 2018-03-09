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
    public class DatabaseHelper
    {

        public string Path { get; }

        public DatabaseHelper(string path)
        {
            Path = path;
            SetupDatabase();
        }

        public void SetupDatabase()
        {
            var conn = new SQLiteConnection(Path);
            conn.CreateTable<Album>();
            /*
            conn.CreateTableAsync<Track>().ContinueWith((results) => 
            {
                Debug.WriteLine("Table Created (Track)");
            });
            conn.CreateTableAsync<Artist>().ContinueWith((results) =>
            {
                Debug.WriteLine("Table Created (Artist)");
            });
            */
        }

        public void AddAlbum(Album a)
        {
            var conn = new SQLiteConnection(Path);
            Console.WriteLine(conn.Insert(a));
        }
        //public void AddTrack(Track t)
        //{
        //    var conn = new SQLiteAsyncConnection(Path);
        //    return conn.Insert(t);
        //}
        //public async Task<int> AddArtist(Artist a)
        //{
        //    var conn = new SQLiteAsyncConnection(Path);
        //    return await conn.InsertAsync(a);
        //}

        public async Task<List<Track>> QueryTracks()
        {
            var conn = new SQLiteAsyncConnection(Path);
            return await conn.QueryAsync<Track>("select * from Track");
        }
        public async Task<List<Album>> QueryAlbums()
        {
            var conn = new SQLiteAsyncConnection(Path);
            return await conn.QueryAsync<Album>("select * from Album");
        }
        public async Task<List<Artist>> QueryArtists()
        {
            var conn = new SQLiteAsyncConnection(Path);
            return await conn.QueryAsync<Artist>("select * from Artist");
        }
    }
}
