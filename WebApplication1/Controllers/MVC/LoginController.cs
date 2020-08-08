using Domain;
using Obligatorio2___WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {

        private ChatPlatformContext db = new ChatPlatformContext();


        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
           

            if (ModelState.IsValid)
            {
                try
                {
                    Usuario usu = db.Usuarios.Where(u => u.Nombre.Equals(usuario.Nombre)).FirstOrDefault();
                    if (usu != null && usu.Password == usuario.Password)
                    {
                        Session["usuario"] = usu.Nombre;                       
                        return RedirectToAction("Index", "DialogosView");

                    }
                    else
                    {
                        ViewBag.Error = "Usuario o contraseña incorrectos";
                        return View(usuario);

                    }

                }
                catch
                {

                    throw;
                }
            }
            else
            {
                ViewBag.Error = "Verifique los datos ingresados e inténtelo nuevamente";
                return View(usuario);

            }
        }

        // GET: Login/CerrarSesion
        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("Create");
        }

        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
