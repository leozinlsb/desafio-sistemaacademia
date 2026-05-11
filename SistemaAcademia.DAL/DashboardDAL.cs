using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAcademia.DAL
{
    public class DashboardDAL
    {
        public DataTable ObterVisaoGeralAlunos()
        {
            DataTable dtVisaoGeral = new DataTable();

            // A famosa query com LEFT JOIN ajustada para a sua tabela Usuario
            string query = @"
                SELECT 
                    U.Id, 
                    U.Nome, 
                    U.Cpf, 
                    U.Telefone, 
                    CASE WHEN MAX(P.Id) IS NOT NULL THEN 'Pago' ELSE 'Sem Pagamento' END AS StatusMesAtual, 
                    MAX(A.DataAcesso) AS UltimoAcesso 
                FROM Usuario U 
                LEFT JOIN Pagamento P ON U.Id = P.UsuarioId AND P.MesReferencia = MONTH(GETDATE()) AND P.AnoReferencia = YEAR(GETDATE())
                LEFT JOIN AcessoCatraca A ON U.Id = A.UsuarioId AND A.Liberado = 1
                WHERE U.IsAdmin = 0
                GROUP BY U.Id, U.Nome, U.Cpf, U.Telefone 
                ORDER BY U.Nome;";

            using (var connection = ConnectionHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    // O SqlDataAdapter é perfeito para preencher DataTables automaticamente
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dtVisaoGeral);
                    }
                }
            }

            return dtVisaoGeral;
        }
    }
}
