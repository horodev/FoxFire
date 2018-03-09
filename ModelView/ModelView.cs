using System;
using PropertyChanged;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxFire
{
    [AddINotifyPropertyChangedInterface]
    public class ModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
