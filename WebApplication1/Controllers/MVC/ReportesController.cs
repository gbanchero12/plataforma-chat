using Domain;
using Obligatorio2___WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers.MVC
{
    public class ReportesController : Controller
    {
        private ChatPlatformContext db = new ChatPlatformContext();
        // GET: Reportes
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
                return RedirectToAction("Index", "No hay mensajes para mostrar");
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
                return RedirectToAction("Index", "No hay usuarios para mostrar");
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
                return RedirectToAction("Index", "No hay conversaciones para administrar");
            }
            return View(ud);
        }


        [HttpPost]
        public ActionResult UsuarioHandler(string idUsuario, string idCliente)
        {
            if (Session["admin"] == null)
                return RedirectToAction("Create", "Login");

            ViewBag.Mensaje = "Se asigno la conversación al usuario seleccionado";
            return View();
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