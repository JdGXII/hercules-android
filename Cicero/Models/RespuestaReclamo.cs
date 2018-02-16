using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Models
{
    public class RespuestaReclamo
    {
        public Reclamo reclamo { get; set; }
        public string poder_representacion_url {get; set;}
        public string apoderado_dni_url { get; set; }
        public string video_respuesta_url { get; set; }
        public string foto_respuesta_url { get; set; }
        string respuesta { get; set; }
    }
}
