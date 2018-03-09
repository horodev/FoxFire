
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
using horodev;

namespace FoxFire
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ApplicationPage CurrentPage { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CurrentPage = ApplicationPage.AlbumGridView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Hi!");
            MediaHandler fetcher = new MediaHandler(new string[] { Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) });
            fetcher.Dictionary.KeyAdded += Dictionary_NewKey;
            Task.Run(async () => {
                await fetcher.CreateDictionary();
            });
        }

        private void Dictionary_NewKey(object sender, KeyAddedEventArgs<Album> e)
        {
            Dispatcher?.Invoke(() =>
            {
                // System.Windows.Controls.Image i = new System.Windows.Controls.Image();
                // i.Source = GetCover(e.Path);
                // Stack.Children.Add(i);
                Label l = new Label();
                l.Content = e.Key.Name;
                Stack.Children.Add(l);
            });
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
