using Domain;
using Obligatorio2___WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
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
                Dialogo dialogo = db.Dialogos.Where(d => d.Cliente.ID == mensaje.Cliente.ID && d.Resuelta == false).FirstOrDefault();



                if (dialogo != null) // dialogo existente
                {
                    mensaje.FechaCreacion = DateTime.Now;
                    mensaje.Usuario = dialogo.Usuario;
                    mensaje.Cliente = dialogo.Cliente;
                    db.Entry(dialogo.Cliente).State = EntityState.Unchanged;
                    db.Entry(mensaje.Usuario).State = EntityState.Unchanged;
                    db.Mensajes.Add(mensaje);
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
                {      //  nuevo dialogo
                    
                    // mejorar algoritmia:
                    Usuario usu_a_asignar = db.Usuarios.Find(UsuarioLibre());

                    mensaje.FechaCreacion = DateTime.Now;
                    mensaje.Usuario = usu_a_asignar;
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
                        Usuario = usu_a_asignar
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

        [HttpGet, Route("~/api/dialogos/buscar-dialogo-by-user")]
        public IHttpActionResult GetDialogos(string usuario)
        {
            if (usuario == null)
                return BadRequest();

            List<Dialogo> dialogos = null;
            try
            {

                dialogos = db.Dialogos.Where(d => d.Usuario.Nombre == usuario).ToList();
                return Ok(dialogos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("~/api/dialogos/buscar-dialogo")]
        public IHttpActionResult GetDialogos(int idDialogo)
        {
            if (idDialogo == 0)
                return BadRequest();
            try
            {

                Dialogo dialogo = db.Dialogos.Find(idDialogo);
                return Ok(dialogo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route("~/api/dialogos/cambiar-usuario")]
        public IHttpActionResult GetDialogos(int idUsuario, int idCliente)
        {
            if (idCliente == 0 || idUsuario == 0)
                return BadRequest();

            try
            {

                Dialogo dialogo = db.Dialogos.Where(d => d.Cliente.ID == idCliente && !d.Resuelta)
                    .FirstOrDefault();

                Usuario a_asignar = db.Usuarios.Find(idUsuario);
                
                if(a_asignar != null && dialogo != null)
                {
                    //dialogo.Mensajes.ForEach(m => m.Usuario = a_asignar);

                    dialogo.Usuario = a_asignar;
                    db.SaveChanges();
                    return Ok(dialogo);
                }

                return BadRequest();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //se deberia de orquestar hacia Usuarios controller
        public int UsuarioLibre()
        {
            List<PairUsu> pair = null;
            PairUsu min = null;
            try
            {
                List<Usuario> usuarios = db.Usuarios.ToList();

                pair = db.Dialogos.GroupBy(x => x.Usuario)
                                  .Select(usu => new PairUsu { UsuarioId = usu.Key.ID, CantidadDeConsultas = usu.Count() }).ToList();
                min = pair.OrderBy(p => p.CantidadDeConsultas).FirstOrDefault();

                usuarios.ForEach(usu =>
                {
                    if (!Contiene(usu.ID, pair))
                    {
                        min = new PairUsu() { UsuarioId = usu.ID };
                    }
                }
                );


            }
            catch (Exception e)
            {
                throw (e);
            }

            return min.UsuarioId;
        }

        private bool Contiene(int id, List<PairUsu> pair)
        {
            foreach (PairUsu p in pair)
            {
                if (p.UsuarioId == id)
                {
                    return true;
                }
            }
            return false;
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
