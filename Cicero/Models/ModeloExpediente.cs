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
        public List<string> demandante_documentos;
        public string dni_respuesta;
        public string respuesta;

        public ModeloExpediente() {
            this.codigo_expediente = "vacio";
        }

        public ModeloExpediente(string codigo_expediente, string demandante_dni, string demandante_email, string demandante_nombre,
            string demandante_direccion, string solicitud, string demandante_comentario, List<string> demandante_documentos)
        {
            this.codigo_expediente = codigo_expediente;
            this.demandante_dni = demandante_dni;
            this.demandante_email = demandante_email;
            this.demandante_nombre_demandado = demandante_nombre;
            this.demandante_direccion_demandado = demandante_direccion;
            this.solicitud = solicitud;
            this.demandante_comentario = demandante_comentario;
            this.demandante_documentos = demandante_documentos;

        }

        public ModeloExpediente(string codigo_expediente, string demandante_dni, string demandante_email, string demandante_nombre,
    string demandante_direccion, string solicitud, string demandante_comentario, List<string> demandante_documentos,
    string dni_respuesta, string respuesta)
        {
            this.codigo_expediente = codigo_expediente;
            this.demandante_dni = demandante_dni;
            this.demandante_email = demandante_email;
            this.demandante_nombre_demandado = demandante_nombre;
            this.demandante_direccion_demandado = demandante_direccion;
            this.solicitud = solicitud;
            this.demandante_comentario = demandante_comentario;
            this.demandante_documentos = demandante_documentos;
            this.dni_respuesta = dni_respuesta;
            this.respuesta = respuesta;

        }
    }
}
