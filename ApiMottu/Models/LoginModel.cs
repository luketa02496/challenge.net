using System.ComponentModel.DataAnnotations;

namespace ApiMottu.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O campo Username é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Password é obrigatório.")]
        [MinLength(4, ErrorMessage = "A senha deve ter pelo menos 4 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }
}
