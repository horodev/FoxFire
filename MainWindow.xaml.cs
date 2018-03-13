
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FoxFire;

namespace FoxFire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ApplicationPage CurrentPage { get; set; }
        public MediaHandler Media { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CurrentPage = ApplicationPage.AlbumGridView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Media = new MediaHandler(new string[] { Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) });
            Task.Run(async () => {
                var needToAdd = await Media.LoadDatabase();
                if(needToAdd)
                {
                    var list = await Media.DatabaseHelper.QueryAlbums();
                    foreach(var a in list)
                    {
                        DEBUG_AddLabel(a.Name);
                    }
                }
            });
            Media.DatabaseHelper.ObjectAdded += DatabaseHelper_ObjectAdded;
        }

        private void DEBUG_AddLabel(string text)
        {
            Dispatcher?.Invoke(() =>
            {
                Label l = new Label();
                l.Content = text;
                Stack.Children.Add(l);
            });
        }

        private void DatabaseHelper_ObjectAdded(object sender, ObjectAddedEventArgs<Album> e)
        {
            DEBUG_AddLabel(e.Object.Name);
        }
        private BitmapImage GetCover(string path)
        {
            var f = TagLib.File.Create(path);
            var Cover = new BitmapImage();
            if(f.Tag.Pictures.Length > 0)
            {
                MemoryStream ms = new MemoryStream(f.Tag.Pictures[0].Data.Data);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                ms.Dispose();

                Bitmap result = new Bitmap(200, 200);

                using (Graphics g = Graphics.FromImage(result))
                {
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    g.DrawImage(img, 0, 0, result.Width, result.Height);
                    g.Dispose();
                }
                using (MemoryStream memory = new MemoryStream())
                {
                    result.Save(memory, ImageFormat.Png);
                    memory.Position = 0;
                    Cover.BeginInit();
                    Cover.StreamSource = memory;
                    Cover.CacheOption = BitmapCacheOption.OnLoad;
                    Cover.EndInit();
                    memory.Dispose();
                }
            }
            return Cover;
        }
    }
}
