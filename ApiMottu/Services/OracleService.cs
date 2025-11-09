using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ApiMottu.Services
{
    public class OracleService
    {
        private readonly string _connectionString;

        public OracleService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleConnection")
                ?? throw new InvalidOperationException("A string de conexão Oracle não foi encontrada no appsettings.json.");
        }

        public async Task<DataTable> ExecuteProcedureAsync(string procedureName, params OracleParameter[] parameters)
        {
            using var connection = new OracleConnection(_connectionString);
            using var command = new OracleCommand(procedureName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null && parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            await connection.OpenAsync();

            using var adapter = new OracleDataAdapter(command);
            var table = new DataTable();
            adapter.Fill(table);

            return table;
        }
    }
}
