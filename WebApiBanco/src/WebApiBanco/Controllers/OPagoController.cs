using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiBanco.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApiBanco.Controllers
{
    [Produces("application/json")]
    [Route("api/OPago")]
    //[Authorize(ActiveAuthenticationSchemes = "Bearer",Roles = "Operador2, Administrador")]
    public class OPagoController : Controller
    {
        private readonly ApplicationDbContext context;
        public OPagoController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IEnumerable<OPago> GetAll()
        {
            return context.OPagos.ToList();
        }
        [HttpGet("{id}", Name = "ordenCreada")]
        public IActionResult GetByIdOp(int id)
        {
            var opago = context.OPagos.FirstOrDefault(x => x.id == id);
            if (opago == null)
            {
                return NotFound();
            }
            return Ok(opago);
        }
        [HttpGet("Sucursal/{id}")]
        public IActionResult GetById(int id)
        {
            var sucursal = context.Sucursales.FirstOrDefault(x => x.id == id);
            if (sucursal == null)
            {
                return NotFound();
            }
            //return Ok(context.OPagos.Where(x => x.SucursalId==id).GroupBy(x=> x.Moneda).ToList());
            //return Ok(context.OPagos.Where(x=> x.SucursalId == id).ToList());
            return Ok(context.OPagos.GroupBy(m => m.MonedaId).Select(x => x.Where(s => s.SucursalId == id).ToList()).ToList());
            
        }
        [HttpPost]
        public IActionResult Post([FromBody] OPago opago)
        {

            if (ModelState.IsValid)
            {
                context.OPagos.Add(opago);
                context.SaveChanges();
                return (new CreatedAtRouteResult("ordenCreada", new { id = opago.id }, opago));
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] OPago opago, int id)
        {
            if (opago.id != id)
            {
                return BadRequest();
            }
            context.Entry(opago).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var opago = context.OPagos.FirstOrDefault(x => x.id == id);
            if (opago == null)
            {
                return NotFound();
            }
            context.OPagos.Remove(opago);
            context.SaveChanges();
            return Ok(opago);
        }
    }

}