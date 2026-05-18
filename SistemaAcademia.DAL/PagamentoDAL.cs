using System;
using Microsoft.Data.SqlClient;
using SistemaAcademia.Models;

namespace SistemaAcademia.DAL
{
    /// <summary>
    /// DAL (Data Access Layer) de PAGAMENTOS.
    /// Responsável por inserir e verificar pagamentos na tabela "Pagamento" do banco.
    /// Chamada pelas classes PagamentoBLL e CatracaBLL.
    /// </summary>
    public class PagamentoDAL
    {
        /// <summary>
        /// Insere um pagamento para o MÊS ATUAL automaticamente.
        /// Usa GETDATE(), MONTH(GETDATE()) e YEAR(GETDATE()) do SQL Server para
        /// preencher data, mês e ano de referência com valores atuais.
        /// Chamado por: PagamentoBLL.RegistrarPagamentoMesVigente() → FormAdmin
        /// </summary>
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

        /// <summary>
        /// Verifica se já existe pagamento para um aluno em determinado mês/ano.
        /// COUNT(1) retorna a quantidade de registros encontrados.
        /// Se > 0, o pagamento já foi feito → evita duplicação.
        /// </summary>
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
                    return count > 0; // true = já existe pagamento nesse mês
                }
            }
        }

        /// <summary>
        /// Registra um pagamento usando um objeto Pagamento completo (com todos os campos).
        /// Diferente de InserirPagamento(), aqui os valores de data, mês e ano vêm
        /// do objeto passado, não do GETDATE() do SQL.
        /// </summary>
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
