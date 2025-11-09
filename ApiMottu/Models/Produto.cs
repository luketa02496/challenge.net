using System.ComponentModel.DataAnnotations;

namespace ApiMottu.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }

        public string? Descricao { get; set; }

        public int Estoque { get; set; }



        public ICollection<PedidoProduto>? PedidoProdutos { get; set; }
    }
}
