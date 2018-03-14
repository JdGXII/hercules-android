using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cicero.DataAccess;
using Cicero.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cicero.Controllers
{
    public class BackendController : Controller
    {
        private string usuario = "cicero";
        private string password = "cicero";
        private bool login = false;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario, string password)
        {
            if(this.usuario == "cicero" && this.password == "cicero")
            {
                this.login = true;
                return RedirectToAction("ListaExpedientes");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    

        private List<ModeloExpediente> getExpedientes()
        {
            List<ModeloExpediente> expedientes = new List<ModeloExpediente>();
            DBConnection testconn = new DBConnection();
            SqlDataReader dataReader = testconn.ReadFromTest("SELECT codigo, email_demandante, nombre_demandado, direccion_demandado, " +
                "comentario_adicional_reclamo, solicitud_reclamo, foto_dni_url," +
                "foto_reclamo_url1, foto_reclamo_url2, foto_reclamo_url3, foto_reclamo_url4 FROM Expedientes");
            while (dataReader.Read())
            {
                
                string codigo = dataReader.GetString(0);
                string email = dataReader.GetString(1);
                string nombre = dataReader.GetString(2);
                string direccion = dataReader.GetString(3);
                string comentario = dataReader.GetString(4);
                string solicitud = dataReader.GetString(5);
                string foto_dni = dataReader.GetString(6);
                string foto_reclamo1 = dataReader.GetString(7);
                string foto_reclamo2 = dataReader.GetString(8);
                string foto_reclamo3 = dataReader.GetString(9);
                string foto_reclamo4 = dataReader.GetString(10);
                List<string> fotos_reclamo = new List<string>();
                fotos_reclamo.Add(foto_reclamo1);
                fotos_reclamo.Add(foto_reclamo2);
                fotos_reclamo.Add(foto_reclamo3);
                fotos_reclamo.Add(foto_reclamo4);


                ModeloExpediente expediente = new ModeloExpediente(codigo, foto_dni, email, nombre, direccion, solicitud, comentario, fotos_reclamo);
                expedientes.Add(expediente);
            }
            testconn.CloseDataReader();
            testconn.CloseConnection();
            return expedientes;
            

        }

        private ModeloExpediente getExpediente(string codigo_expediente)
        {
            ModeloExpediente expediente = new ModeloExpediente();
            DBConnection testconn = new DBConnection();
            SqlDataReader dataReader = testconn.ReadFromTest($"SELECT codigo, email_demandante, nombre_demandado, direccion_demandado, " +
                $"comentario_adicional_reclamo, solicitud_reclamo, foto_dni_url, " +
                $"fapoderado_dni_url, respuesta" +
                $"foto_reclamo_url1, foto_reclamo_url2, foto_reclamo_url3, foto_reclamo_url4 FROM Expedientes WHERE codigo = '{codigo_expediente}'");
            while (dataReader.Read())
            {
                
                
                string codigo = dataReader.GetString(0);
                string email = dataReader.GetString(1);
                string nombre = dataReader.GetString(2);
                string direccion = dataReader.GetString(3);
                string comentario = dataReader.GetString(4);
                string solicitud = dataReader.GetString(5);
                string foto_dni = dataReader.GetString(6);
                string foto_reclamo1 = dataReader.GetString(9);
                string foto_reclamo2 = dataReader.GetString(10);
                string foto_reclamo3 = dataReader.GetString(11);
                string foto_reclamo4 = dataReader.GetString(12);
                string dni_respuesta = "No hay respuesta aun";
                string respuesta = "No hay respuesta aun";
                List<string> fotos_reclamo = new List<string>();
                fotos_reclamo.Add(foto_reclamo1);
                fotos_reclamo.Add(foto_reclamo2);
                fotos_reclamo.Add(foto_reclamo3);
                fotos_reclamo.Add(foto_reclamo4);

                if (!dataReader.IsDBNull(9))
                {
                    dni_respuesta = dataReader.GetString(9);
                }
                if (!dataReader.IsDBNull(10))
                {
                    respuesta = dataReader.GetString(10);
                }

                expediente = new ModeloExpediente(codigo, foto_dni, email, nombre, direccion, solicitud, comentario, fotos_reclamo, dni_respuesta, respuesta);
                
            }
            testconn.CloseDataReader();
            testconn.CloseConnection();
            return expediente;


        }

        public IActionResult ListaExpedientes()
        {
            var modelo = getExpedientes();
            return View(modelo);
            /*if (login)
            {

            }
            else
            {
                return RedirectToAction("Index");
            }*/
        }

        public IActionResult VerExpediente(string id)
        {
            var modelo = getExpediente(id);
            return View(modelo);
            /*if (login)
            {

            }
            else
            {
                return RedirectToAction("Index");
            }*/
        }
    }
    
}