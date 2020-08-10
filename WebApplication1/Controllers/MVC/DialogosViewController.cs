using Domain;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Obligatorio2___WebApi.Models;
using System.Configuration;

namespace WebApplication1.Controllers.MVC
{
    public class DialogosViewController : Controller
    {
        private ChatPlatformContext db = new ChatPlatformContext();

        //local || urlApiPlataformaChat
        private string _url = ConfigurationManager.AppSettings["urlApiPlataformaChat"];
        private string _api = ConfigurationManager.AppSettings["external"];
        // GET: DialogosView
        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Create","Login");

            List<Dialogo> dialogos = null;
            _ = new HttpClient();
            Uri uri = new Uri(_url + "/api/dialogos/buscar-dialogo-by-user"
                + "?" + "usuario=" + Session["Usuario"].ToString());
            HttpClient cliente = new HttpClient();

            Task<HttpResponseMessage> tarea = cliente.GetAsync(uri);
            tarea.Wait();

            if (tarea.Result.IsSuccessStatusCode)
            {
                Task<string> tarea2 = tarea.Result.Content.ReadAsStringAsync();
                tarea2.Wait();

                string json = tarea2.Result;
                dialogos = JsonConvert.DeserializeObject<List<Dialogo>>(json);

            }
            else
            {
                ViewBag.Error = tarea.Result.StatusCode;
            }

           
            
            return View(dialogos);
        }

        // GET: DialogosView/Details/5
        public ActionResult Details(int id)
        {
            if (Session["Usuario"] == null)
                { return RedirectToAction("Create", "Login"); }

            Dialogo buscado = db.Dialogos.Find(id);
            if (buscado == null) { RedirectToAction("Index"); }
            ViewBag.Api = _api;
            ViewBag.Url = _url;
            return View(buscado);
        }

        // GET: DialogosView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DialogosView/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DialogosView/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DialogosView/Edit/5
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

        // GET: DialogosView/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DialogosView/Delete/5
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
