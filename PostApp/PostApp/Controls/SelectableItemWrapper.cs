using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Controls
{
    public class SelectableItemWrapper<T> : INotifyPropertyChanged
    {
        private bool _selected = false;
        public bool IsSelected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
        public T Item { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
