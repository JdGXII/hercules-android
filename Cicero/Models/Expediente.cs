using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Cicero.Models
{
    public class Expediente
    {
        private Random rnd;
        public int codigo_expediente { get; set; }  
        public Reclamo reclamo { get; set; }
        public RespuestaReclamo respuesta_reclamo { get; set; }
        public string laudo { get; set; }
        public readonly bool demandante_acepta_clausula_arbitral;
        public readonly bool demandado_acepta_clausula_arbitral;

        public Expediente()
        {
            rnd = new Random();
            codigo_expediente = rnd.Next();
        }






 
    }
}
