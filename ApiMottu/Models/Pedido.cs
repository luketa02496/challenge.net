using ApiMottu.Models;

public class Pedido
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public decimal ValorTotal { get; set; }

    public int UsuarioId { get; set; }          
    public Usuario Usuario { get; set; } = null!;

    public ICollection<PedidoProduto> PedidoProdutos { get; set; } = new List<PedidoProduto>();
}
