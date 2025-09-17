using ApiMottu.Models;
using System.Linq;

namespace ApiMottu.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            // Garante que o banco exista (mesmo no InMemory)
            context.Database.EnsureCreated();

            // Se já tiver qualquer dado, não repete
            if (context.Usuarios.Any() || context.Produtos.Any() || context.Motos.Any())
                return;

            // --- Usuários ---
            var usuarios = new Usuario[]
            {
                new Usuario { Nome = "Lucas", Email = "lucas@email.com", Senha = "123456" },
                new Usuario { Nome = "Marcella", Email = "marcella@email.com", Senha = "abcdef" }
            };
            context.Usuarios.AddRange(usuarios);

            // --- Produtos ---
            var produtos = new Produto[]
            {
                new Produto { Nome = "Capacete", Descricao = "Capacete de segurança", Preco = 250.00m, Estoque = 10 },
                new Produto { Nome = "Jaqueta", Descricao = "Jaqueta de couro", Preco = 400.00m, Estoque = 5 },
                new Produto { Nome = "Luvas", Descricao = "Par de luvas resistentes", Preco = 100.00m, Estoque = 20 }
            };
            context.Produtos.AddRange(produtos);

            // --- Motos ---
            var motos = new Moto[]
            {
                new Moto { Placa = "ABC1234", Modelo = "Honda CG 160", Status = "Disponível", Localizacao = "Pátio A" },
                new Moto { Placa = "XYZ5678", Modelo = "Yamaha MT-03", Status = "Em manutenção", Localizacao = "Pátio B" },
                new Moto { Placa = "JKL9012", Modelo = "Suzuki GSX", Status = "Alugada", Localizacao = "Pátio C" }
            };
            context.Motos.AddRange(motos);

            // --- Pedido de exemplo ---
            var pedido = new Pedido
            {
                Data = DateTime.Now,
                Usuario = usuarios[0],
                ValorTotal = 350.00m,
                PedidoProdutos = new List<PedidoProduto>
                {
                    new PedidoProduto { Produto = produtos[0], Quantidade = 1 }, // Capacete
                    new PedidoProduto { Produto = produtos[2], Quantidade = 1 }  // Luvas
                }
            };
            context.Pedidos.Add(pedido);

            // --- Salvar tudo ---
            context.SaveChanges();
        }
    }
}
