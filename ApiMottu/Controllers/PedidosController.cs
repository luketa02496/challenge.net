using ApiMottu.Data;
using ApiMottu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;

namespace ApiMottu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

       

// GET: api/pedidos?page=1&pageSize=10
[HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetPedidos(int page = 1, int pageSize = 10)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.PedidoProdutos)
                    .ThenInclude(pp => pp.Produto)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Projeta o resultado para incluir apenas informações relevantes
            var resultado = pedidos.Select(p => new
            {
                p.Id,
                Usuario = new { p.Usuario.Id, p.Usuario.Nome, p.Usuario.Email },
                p.Data,
                p.ValorTotal,
                Produtos = p.PedidoProdutos.Select(pp => new
                {
                    pp.Produto.Id,
                    pp.Produto.Nome,
                    pp.Produto.Preco,
                    pp.Quantidade
                })
            });

            return Ok(resultado);
        }

        // GET: api/pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.PedidoProdutos)
                    .ThenInclude(pp => pp.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            return pedido;
        }

        // POST: api/pedidos
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido([FromBody] Pedido pedido)
        {
            var usuario = await _context.Usuarios.FindAsync(pedido.UsuarioId);
            if (usuario == null)
                return BadRequest("Usuário não encontrado.");

            // Cria a lista de PedidoProdutos
            var pedidoProdutos = new List<PedidoProduto>();
            foreach (var pp in pedido.PedidoProdutos)
            {
                var produto = await _context.Produtos.FindAsync(pp.ProdutoId);
                if (produto == null)
                    return BadRequest($"Produto com Id {pp.ProdutoId} não encontrado.");

                if (pp.Quantidade <= 0)
                    return BadRequest("A quantidade deve ser maior que zero.");

                pedidoProdutos.Add(new PedidoProduto
                {
                    ProdutoId = produto.Id,
                    Quantidade = pp.Quantidade
                });
            }

            pedido.PedidoProdutos = pedidoProdutos;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
        }

        // PUT: api/pedidos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, [FromBody] Pedido pedido)
        {
            if (id != pedido.Id)
                return BadRequest();

            var pedidoExistente = await _context.Pedidos
                .Include(p => p.PedidoProdutos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedidoExistente == null)
                return NotFound();

            pedidoExistente.Data = pedido.Data;
            pedidoExistente.UsuarioId = pedido.UsuarioId;

            // Atualiza PedidoProdutos
            _context.PedidoProdutos.RemoveRange(pedidoExistente.PedidoProdutos);

            var novoPedidoProdutos = new List<PedidoProduto>();
            foreach (var pp in pedido.PedidoProdutos)
            {
                var produto = await _context.Produtos.FindAsync(pp.ProdutoId);
                if (produto == null)
                    return BadRequest($"Produto com Id {pp.ProdutoId} não encontrado.");

                if (pp.Quantidade <= 0)
                    return BadRequest("A quantidade deve ser maior que zero.");

                novoPedidoProdutos.Add(new PedidoProduto
                {
                    PedidoId = pedidoExistente.Id,
                    ProdutoId = produto.Id,
                    Quantidade = pp.Quantidade
                });
            }

            pedidoExistente.PedidoProdutos = novoPedidoProdutos;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/pedidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.PedidoProdutos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            _context.PedidoProdutos.RemoveRange(pedido.PedidoProdutos);
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
