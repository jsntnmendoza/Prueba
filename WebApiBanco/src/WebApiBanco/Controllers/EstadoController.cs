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
    [Route("api/Estado")]
    public class EstadoController : Controller
    {
        private readonly ApplicationDbContext context;
        public EstadoController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IEnumerable<Estado> Get()
        {
            return context.Estados.ToList();
        }
    }
}