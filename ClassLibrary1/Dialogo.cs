using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    }
}