using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBanco.Models
{
    public class Estado
    {
        public int id { get; set; }
        public string nombre { get; set; }
        [JsonIgnore]
        public List<OPago> OPagos { get; set; }
    }
}
