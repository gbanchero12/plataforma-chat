using Domain;
using Obligatorio2___WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class DialogosController : ApiController
    {
        private ChatPlatformContext db = new ChatPlatformContext();

        //recibe un mensaje y lo guarda en un dialogo ya creado 

        [HttpPost, Route("~/api/dialogos/recibir-mensaje")]
        public IHttpActionResult DialogoHandler(Mensaje mensaje)
        {
            if (mensaje == null)
            {
                return BadRequest();
            }

            try
            {
                Dialogo dialogo = db.Dialogos.Where(d => d.Cliente.ID == mensaje.Cliente.ID).SingleOrDefault();
               

                if (dialogo != null) // dialogo existente
                {
                    mensaje.FechaCreacion = DateTime.Now;
                    db.Mensajes.Add(mensaje);
                    db.Entry(mensaje.Cliente).State = EntityState.Unchanged;
                    db.Entry(mensaje.Usuario).State = EntityState.Unchanged;
                    db.SaveChanges();
                    int idMensaje = mensaje.ID;

                    dialogo.Mensajes.Add(mensaje);
                    db.Entry(dialogo.Cliente).State = EntityState.Unchanged;
                    db.Entry(dialogo.Usuario).State = EntityState.Unchanged;
                    db.Dialogos.AddOrUpdate(dialogo);

                    db.SaveChanges();
                    return Ok(dialogo);

                }
                else
                {
                    //  nuevo dialogo
                    mensaje.FechaCreacion = DateTime.Now;
                    db.Mensajes.Add(mensaje);
                    db.Entry(mensaje.Cliente).State = EntityState.Unchanged;
                    db.Entry(mensaje.Usuario).State = EntityState.Unchanged;
                    db.SaveChanges();
                    int idMensaje = mensaje.ID;

                    List<Mensaje> list = new List<Mensaje>();
                    list.Add(mensaje);

                    Dialogo nuevo_dialogo = new Dialogo
                    {
                        Cliente = mensaje.Cliente,
                        Mensajes = list,
                        Resuelta = false,
                        Usuario = mensaje.Usuario
                    };

                    db.Entry(nuevo_dialogo.Cliente).State = EntityState.Unchanged;
                    db.Entry(nuevo_dialogo.Usuario).State = EntityState.Unchanged;
                    db.Dialogos.Add(nuevo_dialogo);

                    db.SaveChanges();
                    return Ok(nuevo_dialogo);

                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
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
