using ApiMottu.Data;
using ApiMottu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        // busca todas os pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource<object>>>> GetPedidos(int page = 1, int pageSize = 10)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.PedidoProdutos)
                    .ThenInclude(pp => pp.Produto)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var resultado = pedidos.Select(p =>
            {
                var pedidoObj = new
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
                };

                var res = new Resource<object>(pedidoObj);
                res.Links.Add(new Link("self", Url.Action(nameof(GetPedido), new { id = p.Id }), "GET"));
                res.Links.Add(new Link("update", Url.Action(nameof(PutPedido), new { id = p.Id }), "PUT"));
                res.Links.Add(new Link("delete", Url.Action(nameof(DeletePedido), new { id = p.Id }), "DELETE"));

                return res;
            });

            return Ok(resultado);
        }

        // GET: api/pedidos/5
        [HttpGet("{id}")]
        //busca uma pedido especifico
        public async Task<ActionResult<Resource<object>>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.PedidoProdutos)
                    .ThenInclude(pp => pp.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            var pedidoObj = new
            {
                pedido.Id,
                Usuario = new { pedido.Usuario.Id, pedido.Usuario.Nome, pedido.Usuario.Email },
                pedido.Data,
                pedido.ValorTotal,
                Produtos = pedido.PedidoProdutos.Select(pp => new
                {
                    pp.Produto.Id,
                    pp.Produto.Nome,
                    pp.Produto.Preco,
                    pp.Quantidade
                })
            };

            var resource = new Resource<object>(pedidoObj);
            resource.Links.Add(new Link("self", Url.Action(nameof(GetPedido), new { id = pedido.Id }), "GET"));
            resource.Links.Add(new Link("update", Url.Action(nameof(PutPedido), new { id = pedido.Id }), "PUT"));
            resource.Links.Add(new Link("delete", Url.Action(nameof(DeletePedido), new { id = pedido.Id }), "DELETE"));
            resource.Links.Add(new Link("all", Url.Action(nameof(GetPedidos)), "GET"));

            return Ok(resource);
        }

        // POST: api/pedidos
        // cadastra um novo pedido
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido([FromBody] Pedido pedido)
        {
            var usuario = await _context.Usuarios.FindAsync(pedido.UsuarioId);
            if (usuario == null)
                return BadRequest("Usuário não encontrado.");

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
        // atualiza um pedido existente
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
        //deleta um pedido
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
