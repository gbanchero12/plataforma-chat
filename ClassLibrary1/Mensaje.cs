using Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{

    [Table("Mensajes")]
    public class Mensaje
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(300)]
        public string Texto { get; set; }

        public virtual DateTime FechaCreacion { get; set; }

        public Cliente Cliente { get; set; }

        public Usuario Usuario { get; set; }

        public Origen OrigenEmisor { get; set; }

        public Mensaje() { }

     


    }
    public enum Origen {
        Cliente,
        Usuario
    }
}



