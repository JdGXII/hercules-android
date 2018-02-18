using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cicero.Models;

namespace Cicero.Controllers
{
    public class HomeController : Controller
    {
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

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
