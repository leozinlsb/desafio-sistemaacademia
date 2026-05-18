using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaAcademia.DAL
{
    /// <summary>
    /// DAL do DASHBOARD ADMINISTRATIVO.
    /// Busca a visão geral dos alunos combinando dados de Usuario, Pagamento e AcessoCatraca.
    /// O resultado alimenta a DataGridView da tela admin (FormAdmin).
    /// Chamada pela: DashboardBLL → FormAdmin
    /// </summary>
    public class DashboardDAL
    {
        /// <summary>
        /// Busca todos os alunos com status de pagamento do mês e último acesso na catraca.
        /// Usa LEFT JOIN para incluir alunos sem pagamento/acesso.
        /// WHERE IsAdmin = 0 filtra apenas alunos (não admins).
        /// GROUP BY agrupa para uma linha por aluno. ORDER BY ordena por nome.
        /// </summary>
        public DataTable ObterVisaoGeralAlunos()
        {
            // DataTable: tabela em memória, ideal para DataGridView do Windows Forms
            DataTable dtVisaoGeral = new DataTable();

            // Query com LEFT JOIN para trazer TODOS os alunos, mesmo sem pagamento ou acesso.
            // CASE WHEN: "if" do SQL → se achou pagamento mostra "Pago", senão "Sem Pagamento"
            // MAX(A.DataAcesso): pega a data mais recente de acesso liberado na catraca
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
                    // SqlDataAdapter + Fill: executa a query e preenche o DataTable automaticamente
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
