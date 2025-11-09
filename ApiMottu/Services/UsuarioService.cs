using System.Data;
using Oracle.ManagedDataAccess.Client;
using ApiMottu.Models;

namespace ApiMottu.Services
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDb")
                ?? throw new Exception("String de conexão 'OracleDb' não encontrada no appsettings.json!");
        }

        public Usuario? ObterUsuarioPorEmail(string email)
        {
            using var conn = new OracleConnection(_connectionString);
            conn.Open();

            using var cmd = new OracleCommand(
                "SELECT ID_USUARIO, NOME, TIPO_USUARIO, EMAIL FROM USUARIO WHERE EMAIL = :email",
                conn
            );
            cmd.Parameters.Add(new OracleParameter("email", email));

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id= Convert.ToInt32(reader["ID_USUARIO"]),
                    Nome = reader["NOME"].ToString() ?? string.Empty,
                    TipoUsuario = reader["TIPO_USUARIO"].ToString() ?? string.Empty,
                    Email = reader["EMAIL"].ToString() ?? string.Empty
                };
            }

            return null;
        }
    }
}
