using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FoxFire
{
    /// <summary>
    /// Interaction logic for AlbumGridPage.xaml
    /// </summary>
    public partial class AlbumGridPage : Page
    {
        public AlbumGridPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Hi!");
            MediaHandler fetcher = new MediaHandler(new string[] { Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) });
        }
    }
}
