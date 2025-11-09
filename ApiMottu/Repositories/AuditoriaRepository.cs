using ApiMottu.Services;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ApiMottu.Repositories
{
    public class AuditoriaRepository
    {
        private readonly OracleService _oracleService;

        public AuditoriaRepository(OracleService oracleService)
        {
            _oracleService = oracleService;
        }

        public async Task<List<dynamic>> ListarAuditoriaAsync(int limit = 100)
        {
            var parameters = new[]
            {
                new OracleParameter("p_limit", OracleDbType.Int32, limit, ParameterDirection.Input),
                new OracleParameter("p_rc", OracleDbType.RefCursor, ParameterDirection.Output)
            };

            var result = await _oracleService.ExecuteProcedureAsync("pkg_auditoria.prc_listar_auditoria_cursor", parameters);

            var auditoria = new List<dynamic>();

            foreach (DataRow row in result.Rows)
            {
                auditoria.Add(new
                {
                    Id = row["ID_AUDITORIA"],
                    Usuario = row["USUARIO"],
                    Operacao = row["OPERACAO"],
                    Data = row["DATA_HORA"],
                    Anteriores = row["VALORES_ANTERIORES"],
                    Novos = row["VALORES_NOVOS"]
                });
            }

            return auditoria;
        }
    }
}
