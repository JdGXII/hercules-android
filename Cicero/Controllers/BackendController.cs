using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Cicero.DataAccess;
using Cicero.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Cicero.Controllers
{
    public class BackendController : Controller
    {
        private string usuario = "cicero";
        private string password = "cicero";
        private bool login = false;

        public IActionResult Index()
        {
            if(TempData["failedlogin"] != null)
            {
                ViewBag.Success = TempData["failedlogin"].ToString();
            }

            return View();
        }

        private bool doLogin(string username, string password)
        {
            bool login = false;
            DBConnection testconn = new DBConnection();

            string query = "Select username, password, access_level FROM Users Where username = @username";
            Dictionary<string, Object> query_params = new Dictionary<string, Object>();
            query_params.Add("@username", username);


            try
            {
                SqlDataReader dataReader;
                dataReader = testconn.ReadFromProduction(query, query_params);


                //if email exists in db
                if (dataReader.Read())
                {
                    //get password from db where it is hashed
                    string password_from_db = dataReader.GetValue(1).ToString();
                    //if password matches, login is succesful
                    if (password == password_from_db)
                    {
                        HttpContext.Session.SetString("username", dataReader.GetValue(0).ToString()); 
                        HttpContext.Session.SetString("access", dataReader.GetValue(2).ToString());


                        //ViewData["sessionString"] = System.Web.HttpContext.Current.Session["userpermission"];
                        testconn.CloseDataReader();
                        testconn.CloseConnection();

                        login = true;


                    }
                }
            }
            catch (Exception e)
            {
                testconn.CloseDataReader();
                testconn.CloseConnection();
                
                login = false;
            }
            testconn.CloseDataReader();
            testconn.CloseConnection();
            return login;

        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            bool login = doLogin(username, password);

            if (login)  //(username == this.username) && (password == this.password))
            {

                return RedirectToAction("ListaExpedientes");

            }
            else
            {

                TempData["failedlogin"] = "Credenciales no reconocidas.";
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
                $"apoderado_dni_url, respuesta," +
                $"foto_reclamo_url1, foto_reclamo_url2, foto_reclamo_url3, foto_reclamo_url4," +
                $"foto_respuesta_url1, foto_respuesta_url2, foto_respuesta_url3, foto_respuesta_url4," +
                $"poder_representacion_foto_url FROM Expedientes WHERE codigo = '{codigo_expediente}'");
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

                string foto_respuesta1 = "#";
                string foto_respuesta2 = "#";
                string foto_respuesta3 = "#";
                string foto_respuesta4 = "#";

                string respuesta_poder = "#";

                List<string> fotos_reclamo = new List<string>();
                fotos_reclamo.Add(foto_reclamo1);
                fotos_reclamo.Add(foto_reclamo2);
                fotos_reclamo.Add(foto_reclamo3);
                fotos_reclamo.Add(foto_reclamo4);

                if (!dataReader.IsDBNull(7))
                {
                    dni_respuesta = dataReader.GetString(7);
                }
                if (!dataReader.IsDBNull(8))
                {
                    respuesta = dataReader.GetString(8);
                }

                if (!dataReader.IsDBNull(13))
                {
                    foto_respuesta1 = dataReader.GetString(13);
                }
                if (!dataReader.IsDBNull(14))
                {
                    foto_respuesta2 = dataReader.GetString(14);
                }
                if (!dataReader.IsDBNull(15))
                {
                    foto_respuesta3 = dataReader.GetString(15);
                }
                if (!dataReader.IsDBNull(16))
                {
                    foto_respuesta4 = dataReader.GetString(16);
                }
                if (!dataReader.IsDBNull(17))
                {
                    respuesta_poder = dataReader.GetString(17);
                }

                List<string> fotos_respuesta = new List<string>();
                fotos_respuesta.Add(foto_respuesta1);
                fotos_respuesta.Add(foto_respuesta2);
                fotos_respuesta.Add(foto_respuesta3);
                fotos_respuesta.Add(foto_respuesta4);



                expediente = new ModeloExpediente(codigo, foto_dni, email, nombre, direccion, 
                    solicitud, comentario, fotos_reclamo, dni_respuesta, respuesta, fotos_respuesta, respuesta_poder);
                
            }
            testconn.CloseDataReader();
            testconn.CloseConnection();
            return expediente;


        }

        public IActionResult ListaExpedientes()
        {
            if(HttpContext.Session.GetString("access") != null)
            {
                if (HttpContext.Session.GetString("access") == "3")
                {
                    var modelo = getExpedientes();
                    return View(modelo);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }        
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult VerExpediente(string id)
        {
            if (HttpContext.Session.GetString("access") != null)
            {
                if (HttpContext.Session.GetString("access") == "3")
                {
                    var modelo = getExpediente(id);
                    return View(modelo);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
    }
    
}