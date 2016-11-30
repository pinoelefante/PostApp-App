using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Api.Data
{
    public class Scuola : INotifyPropertyChanged
    {
        public int id { get; set; }
        private string _nome, _immagine;
        public string nome { get { return _nome; } set { Set(ref _nome, value); } }
        public string localita { get; set; }
        public string email { get; set; }
        public string indirizzo { get; set; }
        public string immagine { get { return _immagine; } set { Set(ref _immagine, value); } }
        public string ruolo { get; set; } //ruolo dell'utente nella scuola

        public string immagineThumb
        {
            get
            {
                if (string.IsNullOrEmpty(immagine))
                    return null;
                return $"thumb.{immagine}";
            }
        }
        public string immagineFull
        {
            get
            {
                if (string.IsNullOrEmpty(immagine))
                    return null;
                return $"{immagine}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Set<T>(ref T _p, T value, [CallerMemberName]string property = "")
        {
            _p = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
