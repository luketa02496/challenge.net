namespace ApiMottu.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = "Cliente";
        public DateTime DataCadastro { get; set; } = DateTime.Now;

       
        public ICollection<Pedido>? Pedidos { get; set; }
    }
}
