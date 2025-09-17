namespace ApiMottu.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;
        public decimal ValorTotal { get; set; }

        // Relacionamento N:N com payload (Quantidade)
        public ICollection<PedidoProduto> PedidoProdutos { get; set; } = new List<PedidoProduto>();
    }
}
