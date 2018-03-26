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
    [Route("api/Sucursal")]
    //[Authorize(ActiveAuthenticationSchemes = "Bearer",Roles = "Operador1, Administrador")]
    public class SucursalController : Controller
    {
        private readonly ApplicationDbContext context;
        public SucursalController(ApplicationDbContext context)
        {
            this.context = context;
        }
        //[HttpGet]
        //public IEnumerable<Sucursal> GetAll(int Bancoid)
        //{
        //    return context.Sucursales.Where(x => x.Bancoid==Bancoid).ToList();
        //}
        [HttpGet]
        public IEnumerable<Sucursal> Get()
        {
            return context.Sucursales.ToList();
        }
        [HttpGet("{id}", Name = "sucursalCreada")]
        public IActionResult GetById(int id)
        {
            var sucursal = context.Sucursales.FirstOrDefault(x => x.id == id);
            if (sucursal == null)
            {
                return NotFound();
            }
            return Ok(sucursal);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Sucursal sucursal)
        {

            if (ModelState.IsValid)
            {
                context.Sucursales.Add(sucursal);
                context.SaveChanges();
                return (new CreatedAtRouteResult("sucursalCreada", new { id = sucursal.id }, sucursal));
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Sucursal sucursal, int id)
        {
            if (sucursal.id != id)
            {
                return BadRequest();
            }
            context.Entry(sucursal).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var sucursal = context.Sucursales.FirstOrDefault(x => x.id == id);
            if (sucursal == null)
            {
                return NotFound();
            }
            context.Sucursales.Remove(sucursal);
            context.SaveChanges();
            return Ok(sucursal);
        }
    }
}