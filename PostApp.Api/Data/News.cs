using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Api.Data
{
    public class News
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
        public int thankyou { get; set; }
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
                return testo.Substring(0, testo.Length / 2) + "...\n[Apri la news per continuare a leggere]";
            }
        }
    }
}
