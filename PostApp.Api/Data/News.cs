using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PostApp.Api.Data
{
    public class News : INotifyPropertyChanged
    {
        public int id { get; set; }
        public string testo { get; set; }
        public string titolo { get; set; }
        public DateTime data { get; set; }
        public string immagine { get; set; }
        public string posizione { get; set; }
        public NewsType tipoNews { get; set; }
        public string publisherNome { get; set; }
        public int publisherId { get; set; }
        public int letta { get; set; }
        private int _thankyou;
        public int thankyou { get { return _thankyou; } set { Set(ref _thankyou, value); } }
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
        public string testoAnteprima
        {
            get
            {
                if (testo.Length < 100)
                    return testo.Substring(0, testo.Length - testo.Length / 8) + "...";
                else 
                    return testo.Substring(0, 100) + "...";
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void Set<T>(ref T k, T v, [CallerMemberName]string property = "")
        {
            k = v;
            if (!string.IsNullOrEmpty(property))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
