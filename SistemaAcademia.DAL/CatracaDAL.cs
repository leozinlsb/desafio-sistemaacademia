using System;
using Microsoft.Data.SqlClient;
using SistemaAcademia.Models;

namespace SistemaAcademia.DAL
{
    public class CatracaDAL
    {
        public void RegistrarLogAcesso(string cpf, bool liberado)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                string query = @"
                    DECLARE @UsuarioId INT;
                    SELECT @UsuarioId = Id FROM Usuario WHERE Cpf = @Cpf;
                    
                    IF @UsuarioId IS NOT NULL
                    BEGIN
                        INSERT INTO AcessoCatraca (UsuarioId, DataAcesso, Liberado) 
                        VALUES (@UsuarioId, GETDATE(), @Liberado);
                    END";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Cpf", cpf);
                    command.Parameters.AddWithValue("@Liberado", liberado);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool VerificarPagamentoAtivo(string cpf)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM Pagamento P
                    INNER JOIN Usuario U ON P.UsuarioId = U.Id
                    WHERE U.Cpf = @Cpf 
                    AND MONTH(P.DataPagamento) = MONTH(GETDATE())
                    AND YEAR(P.DataPagamento) = YEAR(GETDATE())";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Cpf", cpf);
                    connection.Open();

                    int pagamentosEncontrados = Convert.ToInt32(command.ExecuteScalar());
                    return pagamentosEncontrados > 0;
                }
            }
        }
    }
}
