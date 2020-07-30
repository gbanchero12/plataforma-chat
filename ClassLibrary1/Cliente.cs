using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("Clientes")]
    public class Cliente 

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public bool BotActivo { get; set; }

        [MaxLength(10)]
        public string ChatId { get; set; }

        public Cliente()
        {
        }
        public Cliente(Cliente cliente)
        {
            this.BotActivo = cliente.BotActivo;

            this.ChatId = cliente.ChatId;

            this.ID = cliente.ID;
        }
        


    }
}
