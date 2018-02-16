using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Models
{
    public class Reclamo
    {
        public Demandante demandante { get; set; }
        public Demandado demandado { get; set; }
        public string video_url { get; set; }
        public string foto_url { get; set; }
        public string dni_url { get; set; }
        public string solicitd { get; set; }
        public string comentario_adicional { get; set; }
        public string status { get; set; }
        
    }
}
