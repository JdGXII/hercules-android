using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cicero.DataAccess;
using Cicero.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cicero.Controllers
{
    public class MainController : Controller
    {
        private IHostingEnvironment _environment;
        private readonly ILogger _logger;


        public MainController(IHostingEnvironment environment, ILogger<MainController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult PresentarReclamo(IFormFile demandante_dni, string demandante_email , string demandante_nombre_demandado, string demandante_direccion_demandado, string demandante_comentario,  string solicitud, IFormFile demandante_video, IFormFile demandante_documentos)
        {

            //Task<bool> video = Savefile(video_demandante, "videos", video_demandante.FileName);
            
            
            Expediente expediente = new Expediente();
            DBConnection testconn = new DBConnection();
            

            StringBuilder nombre_foto = new StringBuilder(expediente.codigo_expediente.ToString());
            nombre_foto.Append("_foto_dni_demandante.jpg");
            StringBuilder foto_dni_url = new StringBuilder("~/images/dnis/");
            foto_dni_url.Append(nombre_foto);
            Task<bool> imagen = Savefile(demandante_dni, "images/dnis", nombre_foto.ToString());
            StringBuilder video_url = new StringBuilder("~/videos/");
            video_url.Append(expediente.codigo_expediente);
            video_url.Append(demandante_video.FileName);
            StringBuilder foto_extra_url = new StringBuilder("~/images/");
            foto_extra_url.Append(expediente.codigo_expediente);
            foto_extra_url.Append(demandante_documentos.FileName);

            var query = $"INSERT INTO Expedientes(codigo, email_demandante, nombre_demandado, direccion_demandado, comentario_adicional_reclamo, solicitud_reclamo, foto_dni_url, foto_reclamo_url, video_reclamo_url, demandante_acepta_terminos) " +
                $"VALUES ('{expediente.codigo_expediente}', '{demandante_email}', '{demandante_nombre_demandado}', '{demandante_direccion_demandado}', '{demandante_comentario}', '{solicitud}', '{foto_dni_url}', '{foto_extra_url}', '{video_url}', 1)";


            bool v = testconn.WriteToTest(query);
            testconn.CloseConnection();

            TempData["codigo_expediente"] = expediente.codigo_expediente;
            TempData["email"] = demandante_email;
            return RedirectToAction("NumeroExpediente", "Home");
        }

        private async Task<bool> Savefile(IFormFile file, string path, string file_name)
        {
            bool success = true;
            var uploads = Path.Combine(_environment.WebRootPath, path);
            if (file.Length > 0)
            {

                using (var fileStream = new FileStream(Path.Combine(uploads, file_name), FileMode.Create))
                {
                    try
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    catch(Exception ex)
                    {
                        success = false;
                        _logger.LogWarning(ex.ToString());
                    }
                    
                }
            }

            return success;

        }

        public IActionResult NumeroExpediente()
        {

            return RedirectToAction("Index", "Home");
        }

        public string getExpediente(string expediente)
        {
            ModeloExpediente exp = buscarExpediente(expediente);
            if(exp.codigo_expediente != "vacio")
            {
                return "<p style='text-align:center'>El status de tu reclamo es: en verificacion</p>";
            }
            else
            {
                return "<p style='text-align:center'>Reclamo no encontrado. Porfavor revise el numero expediente e intente de nuevo</p>";
                
            }
        }

        private ModeloExpediente buscarExpediente(string codigo_expediente)
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
    }
}