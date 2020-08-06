using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DialogosViewModel
    {   
        public virtual Usuario Usuario { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual List<Mensaje> Mensajes { get; set; }

        public bool Resuelta { get; set; }
    }
}