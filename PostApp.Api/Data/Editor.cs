using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Api.Data
{
    public class Editor
    {
        public virtual string nome { get; set; }
        public int id { get; set; }
        public string categoria { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string indirizzo { get; set; }
        public string localita { get; set; }
        public string geo_coordinate { get; set; }
        public string descrizione { get; set; }
        public string immagine { get; set; }
    }
}
