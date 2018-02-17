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
        public IActionResult PresentarReclamo(Demandante demandante, Demandado demandado,  string solicitud, IFormFile video_demandante, IFormFile documentos_demandante)
        {

            //Task<bool> video = Savefile(video_demandante, "videos", video_demandante.FileName);
            
            
            Expediente expediente = new Expediente();
            DBConnection testconn = new DBConnection();

            SqlDataReader dataReader = testconn.ReadFromTest("SELECT * FROM Expedientes");

            StringBuilder nombre_foto = new StringBuilder(expediente.codigo_expediente.ToString());
            nombre_foto.Append("_foto_dni_demandante.jpg");
            StringBuilder foto_dni_url = new StringBuilder("~/images/dnis/");
            foto_dni_url.Append(nombre_foto);
            Task<bool> imagen = Savefile(documentos_demandante, "images/dnis", nombre_foto.ToString());

       



            return RedirectToAction("Lala");
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

        public IActionResult Lala()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}