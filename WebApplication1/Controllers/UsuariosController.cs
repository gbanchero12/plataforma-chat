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
                List<Usuario> usuarios = db.Usuarios.ToList();



                pair = db.Dialogos.GroupBy(x => x.Usuario)
                                  .Select(usu => new PairUsu { UsuarioId = usu.Key.ID, CantidadDeConsultas = usu.Count() }).ToList();
                min = pair.OrderBy(p => p.CantidadDeConsultas).First();

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
                return InternalServerError(e);
            }

            return Ok(min.UsuarioId);
        }

        private bool Contiene(int id, List<PairUsu> pair)
        {
            foreach(PairUsu p in pair)
            {
                if(p.UsuarioId == id)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpGet, Route("~/api/usuarios/login-usuario")]
        public IHttpActionResult LoginUsuario(string nombre, string password)
        {
            if (nombre.Equals("") || password.Equals(""))
                return BadRequest();

            Usuario usuario = null;
            try
            {

                usuario = db.Usuarios.Where(usu => usu.Nombre == nombre).First();

                //decrypt
                if (!password.Equals(usuario.Password))
                {
                    return Unauthorized();
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



