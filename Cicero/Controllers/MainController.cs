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
using Microsoft.WindowsAzure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using Microsoft.Azure; //Namespace for CloudConfigurationManager

namespace Cicero.Controllers
{
    public class MainController : Controller
    {
        private IHostingEnvironment _environment;
        private readonly ILogger _logger;
        private string blob_string = "DefaultEndpointsProtocol=http;AccountName=ciceron;AccountKey=oW6wCmyY6AG/Fq26s4nl6m0Hsnj8tUcG6wVTdm7K4BAbPevcVzkVpACalgN7xRNKTXzVynKGRkzD4QFk4SO77g==;EndpointSuffix=core.windows.net";


        public MainController(IHostingEnvironment environment, ILogger<MainController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult PresentarReclamo(IFormFile demandante_dni, IFormFile demandante_video, IFormFile demandante_documentos, string demandante_email , string demandante_nombre_demandado, string demandante_direccion_demandado, string demandante_comentario,  string solicitud)
        {

            //Task<bool> video = Savefile(video_demandante, "videos", video_demandante.FileName);
            
            
            Expediente expediente = new Expediente();

            StringBuilder nombre_video = new StringBuilder(expediente.codigo_expediente.ToString());
            nombre_video.Append("_video_demandante.mp4");
            StringBuilder video_url = new StringBuilder("https://ciceron.blob.core.windows.net/videosdemandante/");
            video_url.Append(nombre_video);
            Task<bool> video = Savefile(demandante_video, "videosdemandante", nombre_video.ToString());
            

            StringBuilder nombre_foto = new StringBuilder(expediente.codigo_expediente.ToString());
            nombre_foto.Append("_foto_dni_demandante.jpg");
            StringBuilder foto_dni_url = new StringBuilder("https://ciceron.blob.core.windows.net/dnis/");
            foto_dni_url.Append(nombre_foto);
            Task<bool> imagen = Savefile(demandante_dni, "dnis", nombre_foto.ToString());


            StringBuilder nombre_doc = new StringBuilder(expediente.codigo_expediente.ToString());
            nombre_doc.Append("_foto_extra_demandante.jpg");
            StringBuilder foto_extra_url = new StringBuilder("https://ciceron.blob.core.windows.net/fotosextrademandante/");
            foto_extra_url.Append(nombre_doc);
            Task<bool> doc = Savefile(demandante_documentos, "fotosextrademandante", nombre_doc.ToString());


            

            DBConnection testconn = new DBConnection();
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
            //Stream stream
            //await file.CopyToAsync(stream);
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(blob_string);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference(path);

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(file_name);

            

            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());

            bool success = true;

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