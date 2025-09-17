using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMottu.Models
{
    public class PedidoProduto
    {
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        // Quantidade de cada produto no pedido
        public int Quantidade { get; set; }
    }
}
