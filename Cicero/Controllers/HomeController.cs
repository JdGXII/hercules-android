using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cicero.Models;
using Cicero.DataAccess;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Cicero.Controllers
{
    public class HomeController : Controller
    {
        private string blob_string = "DefaultEndpointsProtocol=http;AccountName=ciceron;AccountKey=oW6wCmyY6AG/Fq26s4nl6m0Hsnj8tUcG6wVTdm7K4BAbPevcVzkVpACalgN7xRNKTXzVynKGRkzD4QFk4SO77g==;EndpointSuffix=core.windows.net";


        public IActionResult Index()
        {
           
            return View();
        }

        public IActionResult NumeroExpediente()
        {
            if(TempData["codigo_expediente"] != null)
            {
                ViewBag.Mensaje = $"El número de expediente para tu reclamo es <b>{TempData["codigo_expediente"]}</b>." +
                    $"<br />Esta información ha sido enviada a tu dirección de correo electrónico {TempData["email"]}." +
                    $"<br />No pierdas tu número de expediente pues lo necesitaras para hacerle seguimiento a tu reclamo." +
                    $"<br /><b>Gracias por usar Cicero</b>";
            }
            else
            {
                ViewBag.Mensaje = "Hubo un error y tu expediente no pudo ser generado. Por favor inténtalo de nuevo.";
            }

            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult RevisarExpediente(string numero_expediente)
        {
            ModeloExpediente expediente = buscarExpediente(numero_expediente);
            if(expediente.codigo_expediente != "vacio")
            {
                TempData["codigo_expediente"] = expediente.codigo_expediente;
                return RedirectToAction("LlenarRespuesta");
            }
            else
            {
                TempData["mensaje"] = "Expediente no encontrado.";
                return RedirectToAction("RevisarExpediente");
            }
        }

        public IActionResult RevisarExpediente()
        {
            if (TempData["mensaje"] != null)
            {
                ViewBag.Mensaje = TempData["mensaje"].ToString();
                return View();
            }
            else
            {
                return View();
            }
        }

        private ModeloExpediente buscarExpediente(string codigo_expediente)
        {
            ModeloExpediente expediente = new ModeloExpediente();
            DBConnection testconn = new DBConnection();
            SqlDataReader dataReader = testconn.ReadFromTest($"SELECT codigo, email_demandante, nombre_demandado, direccion_demandado, comentario_adicional_reclamo, " +
                $"solicitud_reclamo, foto_dni_url, " +
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
                string foto_reclamo1 = dataReader.GetString(7);
                string foto_reclamo2 = dataReader.GetString(8);
                string foto_reclamo3 = dataReader.GetString(9);
                string foto_reclamo4 = dataReader.GetString(10);

                List<string> fotos_reclamo = new List<string>();
                fotos_reclamo.Add(foto_reclamo1);
                fotos_reclamo.Add(foto_reclamo2);
                fotos_reclamo.Add(foto_reclamo3);
                fotos_reclamo.Add(foto_reclamo4);

                expediente = new ModeloExpediente(codigo, foto_dni, email, nombre, direccion, solicitud, comentario, fotos_reclamo);

            }
            testconn.CloseDataReader();
            testconn.CloseConnection();
            return expediente;


        }

        [HttpPost]
        public IActionResult SubirRespuestaReclamo(string expediente, IFormFile dni_poder_legal, IFormFile poder_legal, string comentario_demandado)
        {

            StringBuilder nombre_foto = new StringBuilder(expediente);
            nombre_foto.Append("_foto_dni_representante.jpg");
            StringBuilder foto_dni_url = new StringBuilder("https://ciceron.blob.core.windows.net/respuestademandado/");
            foto_dni_url.Append(nombre_foto);
            Task<bool> imagen = Savefile(dni_poder_legal, "respuestademandado", nombre_foto.ToString());

            DBConnection testconn = new DBConnection();
 
            string query = $"UPDATE Expedientes SET respuesta= '{comentario_demandado}', apoderado_dni_url= 'verificando' WHERE codigo = '{expediente}'";
            bool b = testconn.WriteToTest(query);
            if (b)
            {
                TempData["exito"] = $"Respuesta para reclamo {expediente} ha sido enviada";
                return RedirectToAction("RespuestaEnviada");
            }
            else
            {
                TempData["exito"] = $"Algo salio malo. Su respuesta para reclamo {expediente} no pudo ser enviada.";
                return RedirectToAction("RespuestaEnviada");
            }
        }

        public IActionResult RespuestaEnviada()
        {
            ViewBag.exito = TempData["exito"].ToString();
            return View();

        }

        public IActionResult LlenarRespuesta()
        {
            ViewBag.codigo = TempData["codigo_expediente"].ToString();
            return View();

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

    }
}
