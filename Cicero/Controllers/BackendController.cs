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
            if(usuario == "cicero" && password == "cicero")
            {
                login = true;
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
            SqlDataReader dataReader = testconn.ReadFromTest("SELECT codigo, email_demandante, nombre_demandado, direccion_demandado, comentario_adicional_reclamo, solicitud_reclamo, foto_dni_url, foto_reclamo_url, video_reclamo_url FROM Expedientes");
            while (dataReader.Read())
            {
                
                string codigo = dataReader.GetString(0);
                string email = dataReader.GetString(1);
                string nombre = dataReader.GetString(2);
                string direccion = dataReader.GetString(3);
                string comentario = dataReader.GetString(4);
                string solicitud = dataReader.GetString(5);
                string foto_dni = dataReader.GetString(6);
                string foto_reclamo = dataReader.GetString(7);
                string video = dataReader.GetString(8);

                ModeloExpediente expediente = new ModeloExpediente(codigo, foto_dni, email, nombre, direccion, solicitud, comentario, video, foto_reclamo);
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
            SqlDataReader dataReader = testconn.ReadFromTest($"SELECT codigo, email_demandante, nombre_demandado, direccion_demandado, comentario_adicional_reclamo, solicitud_reclamo, foto_dni_url, foto_reclamo_url, video_reclamo_url FROM Expedientes WHERE codigo = '{codigo_expediente}'");
            while (dataReader.Read())
            {
                
                
                string codigo = dataReader.GetString(0);
                string email = dataReader.GetString(1);
                string nombre = dataReader.GetString(2);
                string direccion = dataReader.GetString(3);
                string comentario = dataReader.GetString(4);
                string solicitud = dataReader.GetString(5);
                string foto_dni = dataReader.GetString(6);
                string foto_reclamo = dataReader.GetString(7);
                string video = dataReader.GetString(8);

                expediente = new ModeloExpediente(codigo, foto_dni, email, nombre, direccion, solicitud, comentario, video, foto_reclamo);
                
            }
            testconn.CloseDataReader();
            testconn.CloseConnection();
            return expediente;


        }

        public IActionResult ListaExpedientes()
        {
            if (login)
            {
                var modelo = getExpedientes();
                return View(modelo);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult VerExpediente(string id)
        {
            if (login)
            {
                var modelo = getExpediente(id);
                return View(modelo);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
    
}