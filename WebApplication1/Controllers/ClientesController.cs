using Obligatorio2___WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using Domain;
using System.Web.Routing;
using Microsoft.VisualStudio.Services.DelegatedAuthorization;
using Microsoft.Graph;

using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ResponseType = Microsoft.VisualStudio.Services.DelegatedAuthorization.ResponseType;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using System.Web.Http.Description;
using System.Data.Entity.Migrations;

namespace WebApplication1.Controllers
{
    
    public class ClientesController : ApiController
    {
        private ChatPlatformContext db = new ChatPlatformContext();

        [System.Web.Http.Description.ResponseType(typeof(Cliente)), System.Web.Http.HttpPost, System.Web.Http.Route("~/api/clientes/guardar-cliente")]
        public IHttpActionResult GuardarCliente(Cliente a_guardar)
        {
            if (a_guardar == null)
            {
                return BadRequest();
            }

            Cliente ya_esta_guardado = db.Clientes.Where(cli => cli.ChatId == a_guardar.ChatId).FirstOrDefault();
            if (ya_esta_guardado != null)
            {
                ya_esta_guardado.BotActivo = false;
                db.Clientes.AddOrUpdate(ya_esta_guardado);
                db.SaveChanges();
                return Ok(ya_esta_guardado);
            }

            try
            {
                a_guardar.BotActivo = false;
                db.Clientes.Add(a_guardar);
                db.SaveChanges();

                return Ok(a_guardar);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Get()
        {
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                clientes = db.Clientes.ToList();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(clientes);
        }

        //GET: api/clientes/buscar-por-id
        [System.Web.Http.HttpGet, System.Web.Http.Route("~/api/clientes/buscar-cliente")]
        public IHttpActionResult BuscarCliente(string id)
        {
            Cliente cliente = new Cliente();
            try
            {
                cliente = db.Clientes.Find(id);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(cliente);
        }

        [System.Web.Http.HttpGet, System.Web.Http.Route("~/api/clientes/cliente-por-chat-id")]
        public IHttpActionResult BuscarClientePorChatId(string chatId)
        {
            if(chatId == null)
            {
                return BadRequest();
            }

            Cliente cliente = new Cliente();
            try
            {
                cliente = db.Clientes.Where(c => c.ChatId == chatId).FirstOrDefault();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            if (cliente == null)
                return Ok("Cliente no encontrado");
            
            return Ok(cliente);
        }

        // PUT: api/Clientes/

        [System.Web.Http.Description.ResponseType(typeof(Cliente)), HttpPut, Route("~/api/clientes/desactivar-chatbot-cliente")]
        public IHttpActionResult DesactivarChatbotCliente(Cliente a_desactivar)
        {
            if (a_desactivar == null)
            {
                return BadRequest();
            }

            if (!a_desactivar.BotActivo)
            {
                return Ok(a_desactivar);
            }

            //BUSCO EL CLIENTE ORIGINAL
            Cliente original = db.Clientes.Find(a_desactivar.ID);
            if (original == null) return NotFound();

            try
            {
                original.BotActivo = false;

                db.SaveChanges();

                return Ok(original);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/Clientes/

        [ResponseType(typeof(Cliente)), HttpPut, Route("~/api/clientes/activar-chatbot-cliente")]
        public IHttpActionResult ActvarChatbotCliente(Cliente a_activar)
        {
            if (a_activar == null)
            {
                return BadRequest();
            }

            if (a_activar.BotActivo)
            {
                return Ok(a_activar);
            }

            //BUSCO EL CLIENTE ORIGINAL
            Cliente original = db.Clientes.Find(a_activar.ID);
            if (original == null) return NotFound();

            try
            {
                original.BotActivo = true;

                db.SaveChanges();

                return Ok(original);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
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

        public void Execute(RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }


}

/* //GET: api/importaciones

        

        //GET: api/importaciones
        [HttpGet,Route("~/api/importaciones/buscar-importacion")]
        public IHttpActionResult BuscarImportacion(string id)
        {
            Importacion importacion = new Importacion();
            try
            {
                importacion = db.Importaciones.Find(id);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(importacion);



        }

        // GET: api/importaciones/5
        [ResponseType(typeof(Importacion))]
        public IHttpActionResult Get(string filtro, string clave="")
        {
            try
            {
                List<Importacion> importaciones = new List<Importacion>();
                switch (filtro)
                {
                    case "0":


                        importaciones = db.Importaciones.ToList();


                        break;
                    case "1":

                        importaciones = db.Importaciones.Where(impo => impo.Producto.Codigo.ToUpper() == clave.ToUpper()).ToList();


                        break;
                    case "2":

                        importaciones = db.Importaciones.Where(impo => impo.Producto.Nombre.ToUpper().Contains(clave.ToUpper())).ToList();


                        break;
                    case "3"://Búsqueda por RUT

                        importaciones = db.Importaciones.Where(impo => impo.Producto.Cliente.Rut.ToUpper() == clave.ToUpper()).ToList();


                        break;
                    case "4"://En depósito

                        importaciones = db.Importaciones.Where(impo => impo.FechaSalida == null && DateTime.Compare(DateTime.Now, impo.FechaPrevSalida) < 0).ToList();


                        break;
                    default:


                        importaciones = db.Importaciones.ToList();


                        break;
                }

                return Ok(importaciones);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            
        }


        // PUT: api/Importaciones/5

        [ResponseType(typeof(Importacion))]

        public IHttpActionResult Put(string id, Importacion a_modificar)
        {
            Usuario usu = a_modificar.Usuario == null ? null : db.Usuarios.Find(a_modificar.Usuario.Nombre);
            

            if (id != a_modificar.CodigoImportacion || !ModelState.IsValid || usu == null)
            {
                return BadRequest();
            }

            //BUSCO EL PRODUCTO ORIGINAL PARA EF LE HAGA EL SEGUIMIENTO
            Importacion original = db.Importaciones.Find(id);
            if (original == null) return NotFound();

            try
            {
                original.Matricula = a_modificar.Matricula;
                original.DireccionEntrega = a_modificar.DireccionEntrega;
                original.FechaSalida = a_modificar.FechaSalida;
                original.Usuario = usu;
                
                db.SaveChanges();

                return Ok(original);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }*/


