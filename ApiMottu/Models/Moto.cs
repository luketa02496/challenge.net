using System.ComponentModel.DataAnnotations;

namespace ApiMottu.Models
{
    public class Moto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(10, ErrorMessage = "A placa deve ter no máximo 10 caracteres.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(50, ErrorMessage = "O modelo deve ter no máximo 50 caracteres.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(20, ErrorMessage = "O status deve ter no máximo 20 caracteres.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "A localização é obrigatória.")]
        [StringLength(50, ErrorMessage = "A localização deve ter no máximo 50 caracteres.")]
        public string Localizacao { get; set; }
    
    }
}
