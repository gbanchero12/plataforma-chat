using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain
{
    [Table("Usuarios")]
    public class Usuario 
    {
   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(30)]
        public string Nombre { get; set; }

        [MaxLength(30)]
        public string Password { get; set; }

        public bool Administrador { get; set; }

    }
}