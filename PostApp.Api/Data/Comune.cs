using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Api.Data
{
    public class Comune
    {
        public string istat { get; set; }
        public string comune { get; set; }
        public override string ToString()
        {
            return comune;
        }
    }
}
