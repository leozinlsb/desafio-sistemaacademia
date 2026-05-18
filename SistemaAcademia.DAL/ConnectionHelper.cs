using Microsoft.Data.SqlClient;

namespace SistemaAcademia.DAL
{
    public static class ConnectionHelper
    {
        private const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=AcademiaDB;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
