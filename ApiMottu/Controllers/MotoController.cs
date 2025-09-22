using Microsoft.AspNetCore.Mvc;
using ApiMottu.Models;
using ApiMottu.Data;
using System.Linq;

namespace ApiMottu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MotoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Moto
        // busca todas as motos
        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Motos.ToList());

        // GET: api/Moto/{id}
        // busca uma moto especifica
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var moto = _context.Motos.Find(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        // GET: api/Moto/buscar?status=...&modelo=...
        [HttpGet("buscar")]
        public IActionResult BuscarPorStatusOuModelo([FromQuery] string? status, [FromQuery] string? modelo)
        {
            var query = _context.Motos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(m => m.Status.ToLower().Contains(status.ToLower()));

            if (!string.IsNullOrWhiteSpace(modelo))
                query = query.Where(m => m.Modelo.ToLower().Contains(modelo.ToLower()));

            return Ok(query.ToList());
        }

        // POST: api/Moto
        // cadastra uma nova moto
        [HttpPost]
        public IActionResult Create(Moto moto)
        {
            _context.Motos.Add(moto);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = moto.Id }, moto);
        }

        // PUT: api/Moto/{id}
        // altera alguma moto existente
        [HttpPut("{id}")]
        public IActionResult Update(int id, Moto updated)
        {
            var moto = _context.Motos.Find(id);
            if (moto == null) return NotFound();

            moto.Placa = updated.Placa;
            moto.Modelo = updated.Modelo;
            moto.Status = updated.Status;
            moto.Localizacao = updated.Localizacao;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Moto/{id}
        // deleta alguma moto
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var moto = _context.Motos.Find(id);
            if (moto == null) return NotFound();

            _context.Motos.Remove(moto);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
