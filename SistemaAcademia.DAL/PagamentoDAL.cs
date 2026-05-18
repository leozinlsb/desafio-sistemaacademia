using System;
using Microsoft.Data.SqlClient;
using SistemaAcademia.Models;

namespace SistemaAcademia.DAL
{
    public class PagamentoDAL
    {
        public void InserirPagamento(int usuarioId, decimal valor)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                string query = @"INSERT INTO Pagamento (UsuarioId, DataPagamento, Valor, MesReferencia, AnoReferencia) 
                                 VALUES (@UsuarioId, GETDATE(), @Valor, MONTH(GETDATE()), YEAR(GETDATE()))";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    command.Parameters.AddWithValue("@Valor", valor);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool VerificarPagamentoMesVigente(int usuarioId, int mes, int ano)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                string query = "SELECT COUNT(1) FROM Pagamento WHERE UsuarioId = @UsuarioId AND MesReferencia = @Mes AND AnoReferencia = @Ano";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    command.Parameters.AddWithValue("@Mes", mes);
                    command.Parameters.AddWithValue("@Ano", ano);
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public void RegistrarPagamento(Pagamento pagamento)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                string query = @"INSERT INTO Pagamento (UsuarioId, Valor, DataPagamento, MesReferencia, AnoReferencia)
                                 VALUES (@UsuarioId, @Valor, @DataPagamento, @MesReferencia, @AnoReferencia)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioId", pagamento.UsuarioId);
                    command.Parameters.AddWithValue("@Valor", pagamento.Valor);
                    command.Parameters.AddWithValue("@DataPagamento", pagamento.DataPagamento);
                    command.Parameters.AddWithValue("@MesReferencia", pagamento.MesReferencia);
                    command.Parameters.AddWithValue("@AnoReferencia", pagamento.AnoReferencia);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
