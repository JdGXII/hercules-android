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
using Hercules.Models;


namespace Cicero.Controllers
{
    public class MainController : Controller
    {
        private IHostingEnvironment _environment;
        private readonly ILogger _logger;
        private string blob_string = "DefaultEndpointsProtocol=https;AccountName=hercules;AccountKey=feI/YA9PoxC9tGbO7yQ5KQqrj5vL7M41UakNDkpfht8/KQwrUsqfnMp8yQx0cbj3cwr16mF8sLxA6rpglGJPGQ==;EndpointSuffix=core.windows.net";


        public MainController(IHostingEnvironment environment, ILogger<MainController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        [HttpPost]
        public async void GuardarFotos(ICollection<IFormFile> fotosProblema)
        {

            Expediente expediente = new Expediente();
            List<string> foto_urls = new List<string>();
            foto_urls.Add("vacio");
            foto_urls.Add("vacio");
            foto_urls.Add("vacio");
            foto_urls.Add("vacio");



            int counter = 1;

            foreach (IFormFile foto in fotosProblema)
            {
                String nombre_foto_evidencia = $"{expediente.codigo_expediente}_fotoProblema{counter}.jpg";
                String foto_evidencia_url = $"https://ciceron.blob.core.windows.net/fotosextrademandante/casos/" +
                    $"{expediente.codigo_expediente}/{nombre_foto_evidencia}";
                bool doc = await Savefile(foto, $"fotosCasos/casos/{expediente.codigo_expediente}",
                    nombre_foto_evidencia.ToString());

                foto_urls[counter - 1] = foto_evidencia_url;

                counter++;
            }

        }

        [HttpPost]

        public IActionResult Login(Usuario usuario)
        {



            bool login = DoLogin(usuario);

            if (login)  //(username == this.username) && (password == this.password))
            {

                return RedirectToAction("MyProfile");

            }
            else
            {

                TempData["failedlogin"] = "Sign in failed. Password or email not recognized.";
                return RedirectToAction("Index");
            }



        }

        private bool DoLogin(Usuario usuario)
        {
            bool login = false;
            DBConnection testconn = new DBConnection();

            string query = "Select Nombre, Email, Id, Telefono, Password from Usuarios Where Email = @email";
            Dictionary<string, Object> query_params = new Dictionary<string, Object>();
            query_params.Add("@email", usuario.Email);


            try
            {
                SqlDataReader dataReader;
                dataReader = testconn.ReadFromProduction(query, query_params);


                //if email exists in db
                if (dataReader.Read())
                {
                    //get password from db where it is hashed
                    string dbPassword = dataReader.GetValue(4).ToString();
                    //if password matches, login is succesful
                    if (dbPassword == usuario.Password)
                    {
                        HttpContext.Session.SetString("Telefono", dataReader.GetValue(3).ToString());
                        HttpContext.Session.SetString("username", dataReader.GetValue(0).ToString());
                        HttpContext.Session.SetString("userid", dataReader.GetValue(2).ToString());
                        HttpContext.Session.SetString("email", dataReader.GetValue(1).ToString());

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
                //System.Web.HttpContext.Current.Session["exception"] = e.ToString();
                login = false;
            }
            testconn.CloseDataReader();
            testconn.CloseConnection();
            return login;

        }

        [HttpPost]
        public IActionResult RegisterUser(Usuario user)
        {

            //hash password before inserting in db
            
            DBConnection testconn = new DBConnection();
            string query = "INSERT INTO Usuarios (Nombre, Email, Password, Telefono, Apellido, TarjetaCredito) " +
                "VALUES (@name, @email, @password, @telefono, @apellido, @tarjetaCredito)";
            Dictionary<string, Object> query_params = new Dictionary<string, object>();
            query_params.Add("@name", user.Nombre);
            query_params.Add("@email", user.Email);
            query_params.Add("@password", user.Password);
            query_params.Add("@telefono", user.Telefono);
            query_params.Add("@apellido", user.Apellido);
            query_params.Add("@tarjetaCredito", user.TarjetaCredito);
            testconn.WriteToProduction(query, query_params);
            testconn.CloseConnection();

            bool login = DoLogin(user);
            if (login)
            {
                return RedirectToAction("AccountInfo", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }



        private async Task<bool> Savefile(IFormFile file, string path, string file_name)
        {
            bool success = true;
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

            try
            {
                await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            }
            catch (Exception e)
            {
                success = false;
            }





            return success;


        }

        public IActionResult NumeroExpediente()
        {

            return RedirectToAction("Index", "Home");
        }
    }
}