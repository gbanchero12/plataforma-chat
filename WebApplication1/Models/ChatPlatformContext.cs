using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Domain;


namespace Obligatorio2___WebApi.Models
{
    public class ChatPlatformContext : DbContext
    {
        
        public DbSet<Dialogo> Dialogos { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public ChatPlatformContext() : base("Conexion")
        {

        }

        
    }
}