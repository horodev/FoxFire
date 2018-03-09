using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using horodev;

namespace FoxFire
{
    public class FileCreationEventArgs
    {
        public FileCreationEventArgs(string album, string filepath)
        {
            Album = album;
            Filepath = filepath;
        }
        public string Album { get; private set; }
        public string Filepath { get; private set; }
    }

    public class MediaHandler
    {
        public delegate void FileCreationEventHandler(object sender, FileCreationEventArgs e);
        public event FileCreationEventHandler FileCreation;
        protected void OnFileCreation(string album, string path)
        {
            FileCreation?.Invoke(this, new FileCreationEventArgs(album, path));
        }
        public string[] Locations { get; private set; }
        public MultiValueDictionary<Album, string> Dictionary
        {
            get
            {
                return _dictionary;
            }

            private set
            {
                _dictionary = value;
            }
        }
        private MultiValueDictionary<Album, string> _dictionary;
        private DatabaseHelper _databaseHelper;
        public DatabaseHelper DatabaseHelper
        {
            get { return _databaseHelper; }
            set { _databaseHelper = value; }
        }

        private string[] _validFileExtensions = new string[]
        {
            ".mp3",
            ".ogg",
            ".acc",
            ".wav",
            ".flac"
        };

        public MediaHandler(string[] locations)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "test.db");
            DatabaseHelper = new DatabaseHelper(dbPath);
            Locations = locations;
            Dictionary = new MultiValueDictionary<Album, string>();

            Dictionary.KeyAdded += Dictionary_KeyAdded;
            Dictionary.ItemAdded += Dictionary_ItemAdded;
            FileCreation += FileFetcher_FileCreation;
        }

        private void Dictionary_KeyAdded(object sender, KeyAddedEventArgs<Album> e)
        {
            DatabaseHelper.AddAlbum(e.Key);
        }
        private void Dictionary_ItemAdded(object sender, ItemAddedEventArgs<Album, string> e)
        {
            //Task.Run(async() => await DatabaseHelper.AddTrack(new Track(e.Value)));
        }
        private void FileFetcher_FileCreation(object sender, FileCreationEventArgs e)
        {
            Dictionary.Add(new Album() { Name = e.Album }, e.Filepath);
        }

        public async Task<MultiValueDictionary<Album, string>> CreateDictionary() 
        {
            var List = await LoadFilesAsync(Locations[0]);
            foreach(var file in List)
            {
                TagLib.File tFile = null;
                try
                {
                    tFile = TagLib.File.Create(file);
                    if (!string.IsNullOrEmpty(tFile.Tag.Album))
                    {
                        await Task.Run(() =>
                        {
                            OnFileCreation(tFile.Tag.Album, file);
                        });
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
            return Dictionary;
        }
        [Obsolete("LoadFiles is deprecated, as we're switching to fully Async Code. Use LoadFilesAsync instead.")]
        private List<string> LoadFiles(string path)
        {
            var list = new List<string>();
            if (Directory.GetDirectories(path).Length > 0)
            {
                foreach (var dir in Directory.GetDirectories(path))
                    list.AddRange(LoadFiles(dir));
            }
            list.AddRange(Directory.GetFiles(path));
            return list;
        }
        private async Task<List<string>> LoadFilesAsync(string path) //TODO: Ändere zu Asynchroner Iterativer Aufruf
        {
            var list = new List<string>();
            foreach(var file in Directory.GetFiles(path))
            {
                if (IsValidTrack(file))
                    list.Add(file);
            }
            foreach (var dir in Directory.GetDirectories(path))
            {
                var dummy = await LoadFilesAsync(dir);
                if(dummy != null)
                {
                    list.AddRange(dummy);
                }
            }
            return list.Count == 0 ? null : list;
        }

        private void AddToList(List<string> list, string file)
        {

        }

        private bool IsValidTrack(string file)
        {
            return _validFileExtensions.Contains(Path.GetExtension(file));
        } 
    }
}
