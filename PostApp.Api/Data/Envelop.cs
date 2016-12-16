using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Api.Data
{
    public class Envelop<T>
    {
        public StatusCodes response { get; set; } = StatusCodes.ENVELOP_UNSET;
        public DateTime time { get; set; }
        public T content { get; set; }
    }
}
