using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiBanco.Models;

namespace WebApiBanco.Controllers
{
    [Produces("application/json")]
    [Route("api/Moneda")]
    public class MonedaController : Controller
    {
        private readonly ApplicationDbContext context;
        public MonedaController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IEnumerable<Moneda> Get()
        {
            return context.Monedas.ToList();
        }
    }
}