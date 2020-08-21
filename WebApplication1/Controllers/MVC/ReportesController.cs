using Domain;
using Newtonsoft.Json;
using Obligatorio2___WebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers.MVC
{
    public class ReportesController : Controller
    {
        private ChatPlatformContext db = new ChatPlatformContext();

        //local || urlApiPlataformaChat
        private string _url = ConfigurationManager.AppSettings["urlApiPlataformaChat"];
        private string _api = ConfigurationManager.AppSettings["external"];


        public ActionResult Index(string msj = "")
        {
            ViewBag.Mensaje = msj;
            return View();
        }

        [HttpGet]
        public ActionResult Dialogos()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Create", "Login");

            List<Dialogo> dialogos = db.Dialogos.Where(m => m.Cliente != null && m.Usuario != null).ToList();
            if(dialogos == null)
            {
                //, "No hay mensajes para mostrar"
                return RedirectToAction("Index");
            }
            
            return View(dialogos);
        }

        [HttpGet]
        public ActionResult Usuarios()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Create", "Login");

            List<Usuario> usuarios = db.Usuarios.Select(u => u).ToList();
            if (usuarios == null)
            {
                //, "No hay usuarios para mostrar"
                return RedirectToAction("Index");
            }
            return View(usuarios);
        }

        [HttpGet]
        public ActionResult UsuarioHandler()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Create", "Login");

            UsuarioDialogo ud = new UsuarioDialogo
            {
                Usuarios = db.Usuarios.Select(u => u).ToList(),
                Dialogos = db.Dialogos.Where(d => !d.Resuelta).ToList()
            };

            if (ud.Usuarios == null || ud.Dialogos == null)
            {
                //, "No hay conversaciones para administrar"
                return RedirectToAction("Index");
            }
            return View(ud);
        }


        [HttpPost]
        public ActionResult UsuarioHandler(int idUsuario, int idCliente)
        {
            if (Session["admin"] == null)
                return RedirectToAction("Create", "Login");


            Dialogo dialogo = null;
            _ = new HttpClient();
            Uri uri = new Uri(_url + "/api/dialogos/cambiar-usuario?idUsuario="+idUsuario+"&idCliente="+idCliente);
            HttpClient cliente = new HttpClient();
            var param = new { idUsuario, idCliente };
            Task<HttpResponseMessage> tarea = cliente.PostAsJsonAsync(uri, param);
            tarea.Wait();

            if (tarea.Result.IsSuccessStatusCode)
            {
                Task<string> tarea2 = tarea.Result.Content.ReadAsStringAsync();
                tarea2.Wait();

                string json = tarea2.Result;
                dialogo = JsonConvert.DeserializeObject<Dialogo>(json);

            }
            else
            {
                ViewBag.Error = tarea.Result.StatusCode;
            }
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}