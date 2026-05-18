using System;
using Microsoft.Data.SqlClient;
using SistemaAcademia.Models;

namespace SistemaAcademia.DAL
{
    /// <summary>
    /// DAL (Data Access Layer / Camada de Acesso a Dados) da CATRACA.
    /// 
    /// Esta classe contém os métodos que interagem com o banco de dados
    /// relacionados ao controle de acesso (catraca) da academia:
    /// 
    ///   1. RegistrarLogAcesso()     → Salva no banco o HISTÓRICO de cada tentativa de entrada
    ///   2. VerificarPagamentoAtivo() → Verifica se o aluno tem pagamento no mês atual
    /// 
    /// É chamada pela CatracaBLL (camada de regras de negócio).
    /// </summary>
    public class CatracaDAL
    {
        /// <summary>
        /// REGISTRA UM LOG (histórico) DE ACESSO À CATRACA no banco de dados.
        /// 
        /// Cada vez que um aluno tenta entrar na academia, este método é chamado
        /// para salvar na tabela AcessoCatraca:
        ///   - Quem tentou entrar (buscado pelo CPF)
        ///   - Quando tentou (data/hora atual via GETDATE())
        ///   - Se foi liberado ou não (parâmetro 'liberado')
        /// 
        /// O método usa um bloco SQL com DECLARE para primeiro encontrar o Id do
        /// usuário pelo CPF e depois inserir o registro na tabela AcessoCatraca.
        /// 
        /// Chamado pela: CatracaBLL.VerificarAcessoCatraca()
        /// </summary>
        /// <param name="cpf">CPF do aluno que está tentando passar na catraca</param>
        /// <param name="liberado">true = acesso liberado, false = acesso bloqueado</param>
        public void RegistrarLogAcesso(string cpf, bool liberado)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                // Este bloco SQL faz duas coisas em sequência:
                //   1. DECLARE @UsuarioId: Cria uma variável temporária no SQL
                //   2. SELECT @UsuarioId = Id: Busca o Id do usuário pelo CPF
                //   3. IF @UsuarioId IS NOT NULL: Só insere se o usuário existir
                //   4. INSERT INTO AcessoCatraca: Salva o registro de acesso
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

        /// <summary>
        /// VERIFICA SE O ALUNO TEM PAGAMENTO ATIVO (em dia) para o mês atual.
        /// 
        /// Faz uma consulta no banco usando INNER JOIN entre as tabelas Pagamento e Usuario.
        /// A verificação é feita comparando o mês e ano do pagamento com o mês e ano ATUAIS
        /// (MONTH(GETDATE()) e YEAR(GETDATE())).
        /// 
        /// Se COUNT(*) > 0, significa que existe pelo menos um pagamento para o mês atual → pago!
        /// Se COUNT(*) == 0, o aluno está inadimplente → catraca bloqueada!
        /// 
        /// Chamado pela: CatracaBLL.VerificarAcessoCatraca()
        /// </summary>
        /// <param name="cpf">CPF do aluno a verificar</param>
        /// <returns>true se tem pagamento no mês atual, false se está inadimplente</returns>
        public bool VerificarPagamentoAtivo(string cpf)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                // INNER JOIN: Junta a tabela Pagamento com a tabela Usuario
                //             através da relação P.UsuarioId = U.Id.
                // WHERE U.Cpf = @Cpf: Filtra pelo CPF do aluno.
                // AND MONTH/YEAR: Filtra pelos pagamentos do mês e ano ATUAIS.
                // COUNT(*): Conta quantos pagamentos existem para essas condições.
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

                    // ExecuteScalar: Executa o SQL e retorna apenas o PRIMEIRO valor da primeira
                    // coluna do resultado (neste caso, o COUNT). Ideal para queries que retornam
                    // um único valor numérico.
                    int pagamentosEncontrados = Convert.ToInt32(command.ExecuteScalar());

                    // Se encontrou ao menos 1 pagamento no mês atual, retorna true (pago!)
                    return pagamentosEncontrados > 0;
                }
            }
        }
    }
}
