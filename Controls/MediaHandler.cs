using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using horodev;

namespace FoxFire
{
    public class MediaHandler
    {
        private string[] _validFileExtensions = new string[]
        {
            ".mp3",
            ".ogg",
            ".acc",
            ".wav",
            ".flac"
        };
        public string[] Locations { get; private set; }

        public string DatabasePath { get; set; }

        private DatabaseHelper _databaseHelper;
        public DatabaseHelper DatabaseHelper
        {
            get { return _databaseHelper; }
            set { _databaseHelper = value; }
        }

        public MediaHandler(string[] locations)
        {
            DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "horodev//foxfire//media.db");
            DatabaseHelper = new DatabaseHelper(DatabasePath);
            Locations = locations;
        }

        public async Task<bool> LoadDatabase()
        {
            if (!File.Exists(DatabasePath))
            {
                DatabaseHelper.SetupDatabase();
                await CreateDatabase();
                return false;
            }
            return true;
        }
        private async Task CreateDatabase()
        {
            var List = LoadFiles(Locations[0]);
            foreach (var file in List)
            {
                TagLib.File tFile = null;
                try
                {
                    tFile = TagLib.File.Create(file);
                    if (!string.IsNullOrEmpty(tFile.Tag.Album))
                    {
                        var list = await DatabaseHelper.QueryAlbums(tFile.Tag.Album);
                        if(list.Count == 0)
                        {
                            DatabaseHelper.AddAlbum(new Album() { Name = tFile.Tag.Album });
                        }
                    }
                }
                catch (TagLib.UnsupportedFormatException)
                {
                    Debug.WriteLine($"UnsupportedFormatException thrown with file: {file}");
                }
                finally
                {
                    tFile?.Dispose();
                }
            }
        }
        private List<string> LoadFiles(string path)
        {
            return Directory.EnumerateFiles(path, ".", SearchOption.AllDirectories).Where(s => IsValidTrack(s)).ToList();
        }
        private bool IsValidTrack(string file)
        {
            return _validFileExtensions.Contains(Path.GetExtension(file));
        }
    }
}
