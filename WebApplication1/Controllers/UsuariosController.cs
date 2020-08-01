using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain;
namespace WebApplication1.Controllers
{
    public class UsuariosController : ApiController
    {
        private Obligatorio2___WebApi.Models.ChatPlatformContext db = new Obligatorio2___WebApi.Models.ChatPlatformContext();

        //GET: api/clientes/buscar-por-id
        [HttpGet, Route("~/api/usuarios/usuario-a-asignar")]
        public IHttpActionResult BuscarUsuario()
        {
            List<PairUsu> pair = null;
            PairUsu min = null;
            try
            {
                
                pair = db.Dialogos.GroupBy(x => x.Usuario)
                                  .Select(usu => new PairUsu { UsuarioId = usu.Key.ID, CantidadDeConsultas = usu.Count() }).ToList();
                min = pair.OrderBy(p => p.CantidadDeConsultas).First();

            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(min.UsuarioId);
        }

        [HttpGet, Route("~/api/usuarios/login-usuario")]
        public IHttpActionResult LoginUsuario(string nombre, string password)
        {
            if (nombre.Equals("") || password.Equals(""))
                return BadRequest();

            Usuario usuario = null;
            try
            {

                usuario = db.Usuarios.Where(usu => usu.Nombre == nombre).SingleOrDefault();

                //decrypt
                if (!password.Equals(usuario.Password))
                {
                    return Ok("Contrasena incorrecta");
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(usuario);
        }
    }
}



