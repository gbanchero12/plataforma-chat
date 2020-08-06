using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Domain
{
    [Table("Dialogos")]
    public class Dialogo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual List<Mensaje> Mensajes { get; set; }

        public bool Resuelta { get; set; }

        public static explicit operator Dialogo(Task<Dialogo> v)
        {
            throw new NotImplementedException();
        }
    }
}