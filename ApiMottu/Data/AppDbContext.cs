using Microsoft.EntityFrameworkCore;
using ApiMottu.Models;

namespace ApiMottu.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoProduto> PedidoProdutos { get; set; } // Tabela de junção

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---- MOTOS ----
            modelBuilder.Entity<Moto>(entity =>
            {
                entity.ToTable("MOTOS");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("ID")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Placa)
                      .HasColumnName("PLACA")
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(e => e.Modelo)
                      .HasColumnName("MODELO")
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Status)
                      .HasColumnName("STATUS")
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(e => e.Localizacao)
                      .HasColumnName("LOCALIZACAO")
                      .HasMaxLength(50)
                      .IsRequired();
            });

            // ---- PRODUTOS ----
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.ToTable("PRODUTOS");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nome)
                      .HasColumnName("NOME")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Descricao)
                      .HasColumnName("DESCRICAO")
                      .HasMaxLength(255);

                entity.Property(e => e.Preco)
                      .HasColumnName("PRECO")
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Estoque)
                      .HasColumnName("ESTOQUE");
            });

            // ---- USUÁRIOS ----
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIOS");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nome)
                      .HasColumnName("NOME")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Email)
                      .HasColumnName("EMAIL")
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Senha)
                      .HasColumnName("SENHA")
                      .HasMaxLength(100)
                      .IsRequired();
            });

            // ---- PEDIDOS ----
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("PEDIDOS");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Data)
                      .HasColumnName("DATA")
                      .HasColumnType("datetime");

                entity.Property(e => e.ValorTotal)
                      .HasColumnName("VALOR_TOTAL")
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(p => p.Usuario)
                      .WithMany(u => u.Pedidos)
                      .HasForeignKey(p => p.UsuarioId);
            });

            // ---- PEDIDO_PRODUTOS (TABELA DE JUNÇÃO) ----
            modelBuilder.Entity<PedidoProduto>(entity =>
            {
                entity.ToTable("PEDIDO_PRODUTOS");

                entity.HasKey(pp => new { pp.PedidoId, pp.ProdutoId });

                entity.HasOne(pp => pp.Pedido)
                      .WithMany(p => p.PedidoProdutos)
                      .HasForeignKey(pp => pp.PedidoId);

                entity.HasOne(pp => pp.Produto)
                      .WithMany(pr => pr.PedidoProdutos)
                      .HasForeignKey(pp => pp.ProdutoId);
            });
        }
    }
}
