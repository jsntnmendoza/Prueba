using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBanco.Models
{
    public class Sucursal
    {
        public Sucursal()
        {
            OPagos = new List<OPago>();
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
        public List<OPago> OPagos { get; set; }

        [ForeignKey("Banco")]
        public int Bancoid { get; set; }
        [JsonIgnore]
        public Banco Banco { get; set; }
    }
}
