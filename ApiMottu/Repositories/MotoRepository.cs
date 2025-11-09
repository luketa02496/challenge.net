using ApiMottu.Services;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ApiMottu.Repositories
{
    public class MotoRepository
    {
        private readonly OracleService _oracleService;

        public MotoRepository(OracleService oracleService)
        {
            _oracleService = oracleService;
        }

        public async Task<List<dynamic>> ListarMotosAsync()
        {
            var param = new OracleParameter("p_rc", OracleDbType.RefCursor, ParameterDirection.Output);

            var result = await _oracleService.ExecuteProcedureAsync("pkg_moto.prc_listar_motos_json_cursor", param);

            var motos = new List<dynamic>();

            foreach (DataRow row in result.Rows)
            {
                motos.Add(new
                {
                    IdMoto = row["ID_MOTO"],
                    Placa = row["PLACA"],
                    Modelo = row["MODELO"],
                    Patio = row["PATIO"],
                    Status = row["STATUS"],
                    JsonMoto = row["JSON_MOTO"]
                });
            }

            return motos;
        }
    }
}
