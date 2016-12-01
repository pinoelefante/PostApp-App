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
    }
}
