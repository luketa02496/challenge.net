using Microsoft.AspNetCore.Mvc;
using ApiMottu.Models;
using ApiMottu.Data;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using ApiMottu.Services; 

namespace ApiMottu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly OracleService _oracleService;

        public MotoController(AppDbContext context, OracleService oracleService)
        {
            _context = context;
            _oracleService = oracleService;
        }

        // GET: api/Moto
        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Motos.ToList());

        // GET: api/Moto/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var moto = _context.Motos.Find(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        
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
        [HttpPost]
        public IActionResult Create(Moto moto)
        {
            _context.Motos.Add(moto);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = moto.Id }, moto);
        }

        // PUT: api/Moto/{id}
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
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var moto = _context.Motos.Find(id);
            if (moto == null) return NotFound();

            _context.Motos.Remove(moto);
            _context.SaveChanges();
            return NoContent();
        }

        // NOVO ENDPOINT: chama procedure Oracle
        // GET: api/Moto/oracle/listar
        [HttpGet("oracle/listar")]
        public async Task<IActionResult> ListarMotosOracle()
        {
            try
            {
                var param = new OracleParameter("p_rc", OracleDbType.RefCursor, ParameterDirection.Output);
                var result = await _oracleService.ExecuteProcedureAsync("pkg_moto.prc_listar_motos_json_cursor", param);

                var motos = new List<object>();

                foreach (DataRow row in result.Rows)
                {
                    motos.Add(new
                    {
                        IdMoto = row["ID_MOTO"],
                        Placa = row["PLACA"],
                        Modelo = row["MODELO"],
                        Patio = row["PATIO"],
                        Status = row["STATUS"],
                        JsonMoto = row["JSON_MOTO"]
                    });
                }

                return Ok(motos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar motos no Oracle: {ex.Message}");
            }
        }
    }
}
