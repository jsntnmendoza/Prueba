using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiBanco.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApiBanco.Controllers
{
    [Produces("application/json")]
    [Route("api/Banco")]
    //[Authorize(ActiveAuthenticationSchemes = "Bearer",Roles = "Operador1")]
    public class BancoController : Controller
    {
        
        private readonly ApplicationDbContext context;
        public BancoController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IEnumerable<Banco> Get()
        {
            return context.Bancos.ToList();
        }
        [HttpGet("{id}", Name ="bancoCreado")]
        public IActionResult GetById(int id)
        {
            //var banco = context.Bancos.Include(x => x.Sucursales).FirstOrDefault(x => x.id == id);
            var banco= context.Bancos.FirstOrDefault(x => x.id == id);
            if (banco==null)
            {
                return NotFound();
            }
            return Ok(banco);
        }
        [HttpGet("{id}/Sucursal")]
        public IActionResult GetSucById(int id)
        {
            //var banco = context.Bancos.Include(x => x.Sucursales).FirstOrDefault(x => x.id == id);
            var banco = context.Bancos.FirstOrDefault(x => x.id == id);
            if (banco == null)
            {
                return NotFound();
            }
            return Ok(context.Sucursales.Where(x => x.Bancoid == id).ToList());
        }
        [HttpPost]
        public IActionResult Post([FromBody] Banco banco)
        {
            
            if (ModelState.IsValid)
            {
                context.Bancos.Add(banco);
                context.SaveChanges();
                return (new CreatedAtRouteResult("bancoCreado", new { id = banco.id}, banco));
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Banco banco, int id)
        {
            if (banco.id != id)
            {
                return BadRequest();
            }
            context.Entry(banco).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var banco = context.Bancos.FirstOrDefault(x => x.id==id);
            if (banco == null)
            {
                return NotFound();
            }
            context.Bancos.Remove(banco);
            context.SaveChanges();
            return Ok(banco);
        }
    }
}