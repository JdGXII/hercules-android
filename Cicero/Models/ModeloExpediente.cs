using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Models
{
    public class ModeloExpediente
    {
        public string codigo_expediente;
        public string demandante_dni;
        public string demandante_email;
        public string demandante_nombre_demandado;
        public string demandante_direccion_demandado;
        public string solicitud;
        public string demandante_comentario;
        public string demandante_video;
        public string demandante_documentos;

        public ModeloExpediente() { }

        public ModeloExpediente(string codigo_expediente, string demandante_dni, string demandante_email, string demandante_nombre,
            string demandante_direccion, string solicitud, string demandante_comentario, string demandante_video, string demandante_documentos)
        {
            this.codigo_expediente = codigo_expediente;
            this.demandante_dni = demandante_dni;
            this.demandante_email = demandante_email;
            this.demandante_nombre_demandado = demandante_nombre;
            this.demandante_direccion_demandado = demandante_direccion;
            this.solicitud = solicitud;
            this.demandante_comentario = demandante_comentario;
            this.demandante_video = demandante_video;
            this.demandante_documentos = demandante_documentos;

        }
    }
}
