using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBanco.Models
{
    public class Banco
    {
        public Banco()
        {
            Sucursales = new List<Sucursal>();
        }
        public int id { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public DateTime fechareg
        {
            get
            {
                return this.dateCreated.HasValue
                   ? this.dateCreated.Value
                   : DateTime.Now;
            }

            set { this.dateCreated = value; }
        }

        private DateTime? dateCreated = null;

        public List<Sucursal> Sucursales { get; set; }
    }
}
