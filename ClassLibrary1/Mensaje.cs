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

        [MaxLength(1000)]
        public string Texto { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "0:dd/MM/yy H:mm")]
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



