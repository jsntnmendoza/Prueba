using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBanco.Models
{
    public class OPago
    {
        public int id { get; set; }
        public double monto { get; set; }
        public DateTime fechapag { get; set; }

        [ForeignKey("Moneda")]
        public int MonedaId { get; set;}
        [JsonIgnore]
        public Moneda Moneda { get; set; }

        [ForeignKey("Estado")]
        public int EstadoId { get; set; }
        [JsonIgnore]
        public Estado Estado { get; set; }

        [ForeignKey("Sucursal")]
        public int SucursalId { get; set; }
        [JsonIgnore]
        public Sucursal Sucursal { get; set; }
    }
}
