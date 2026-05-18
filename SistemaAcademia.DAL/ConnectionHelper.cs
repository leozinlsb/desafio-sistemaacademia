using Microsoft.Data.SqlClient;

namespace SistemaAcademia.DAL
{
    /// <summary>
    /// HELPER (auxiliar) DE CONEXÃO COM O BANCO DE DADOS.
    /// 
    /// Esta classe estática centraliza a string de conexão (connection string) do banco 
    /// de dados SQL Server em um ÚNICO lugar. Todas as classes DAL do projeto usam
    /// ConnectionHelper.GetConnection() para obter uma conexão pronta.
    /// 
    /// Vantagem: Se você precisar mudar o servidor, o nome do banco ou a forma de
    /// autenticação, basta alterar AQUI e todo o sistema é afetado automaticamente.
    /// 
    /// A classe é ESTÁTICA (static), o que significa que você não precisa criar um
    /// objeto (instância) para usá-la. Basta chamar ConnectionHelper.GetConnection().
    /// </summary>
    public static class ConnectionHelper
    {
        // Connection String: É a "receita" que diz ao C# COMO se conectar ao banco de dados.
        // Vamos decompor cada parte:
        //
        //   Server=(localdb)\MSSQLLocalDB   → Conecta ao SQL Server LocalDB (versão local instalada com Visual Studio)
        //   Database=AcademiaDB             → Nome do banco de dados que vamos acessar
        //   Integrated Security=True        → Usa autenticação do Windows (não precisa de senha do SQL)
        //   TrustServerCertificate=True     → Aceita o certificado SSL do servidor (evita erro em dev)
        //
        // ALTERE esta string se seu servidor SQL for diferente (ex: .\SQLEXPRESS)
        private const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=AcademiaDB;Integrated Security=True;TrustServerCertificate=True;";

        /// <summary>
        /// Cria e retorna uma NOVA conexão (SqlConnection) com o banco de dados.
        /// Quem chamar este método deve usar "using" para garantir que a conexão
        /// seja fechada e liberada automaticamente após o uso.
        /// 
        /// Exemplo de uso:
        ///   using (var connection = ConnectionHelper.GetConnection())
        ///   {
        ///       connection.Open();
        ///       // ... executa comandos SQL aqui ...
        ///   } // ← A conexão é fechada automaticamente aqui pelo "using"
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
