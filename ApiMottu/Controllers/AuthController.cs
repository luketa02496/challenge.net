using ApiMottu.Models;
using ApiMottu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiMottu.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UsuarioService _usuarioService;

        public AuthController(IConfiguration config, UsuarioService usuarioService)
        {
            _config = config;
            _usuarioService = usuarioService;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public IActionResult Token([FromBody] LoginModel login)
        {
            if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
                return BadRequest("Usuário e senha são obrigatórios.");

            // Buscar o usuário pelo e-mail
            var usuario = _usuarioService.ObterUsuarioPorEmail(login.Username);
            if (usuario == null)
                return Unauthorized("Usuário não encontrado.");

            // Aqui deveria ter a verificação de senha real (criptografada no banco)
            if (login.Password != "123456")
                return Unauthorized("Senha incorreta.");

            // Criar token JWT
            var jwtSection = _config.GetSection("Jwt");
            var keyString = jwtSection["Key"] ?? throw new Exception("JWT Key não configurada!");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim("nome", usuario.Nome),
                new Claim("tipo", usuario.TipoUsuario)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                usuario = new
                {
                    usuario.Nome,
                    usuario.TipoUsuario,
                    usuario.Email
                }
            });
        }
    }
}
