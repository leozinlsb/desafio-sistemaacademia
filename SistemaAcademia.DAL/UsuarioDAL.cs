using System;
using Microsoft.Data.SqlClient;
using SistemaAcademia.Models;

namespace SistemaAcademia.DAL
{
    /// <summary>
    /// DAL (Data Access Layer / Camada de Acesso a Dados) do USUÁRIO.
    /// 
    /// Esta classe é responsável por TODA a comunicação com a tabela "Usuario" no banco 
    /// de dados SQL Server. Ela contém os métodos CRUD:
    ///   C - Create  → Cadastrar()
    ///   R - Read    → BuscarPorLogin(), BuscarPorCpf(), BuscarPorId()
    ///   U - Update  → Atualizar()
    ///   D - Delete  → Excluir()
    /// 
    /// IMPORTANTE: Esta camada NÃO contém regras de negócio (validações, criptografia, etc.).
    /// Ela apenas monta os comandos SQL, envia ao banco e retorna os resultados.
    /// As regras ficam na camada BLL (Business Logic Layer).
    /// 
    /// Padrão usado: ADO.NET com SqlCommand e parâmetros (@Nome, @Cpf, etc.)
    /// Os parâmetros evitam SQL Injection (ataque onde alguém coloca SQL malicioso nos campos).
    /// </summary>
    public class UsuarioDAL
    {
        /// <summary>
        /// ATUALIZA os dados de um usuário já existente no banco.
        /// 
        /// Nota: O CPF e o Login NÃO são atualizados para manter a integridade dos dados.
        /// Se o CPF pudesse ser alterado, poderia causar inconsistências nos pagamentos
        /// e acessos de catraca que referenciam o usuário pelo Id/CPF.
        /// 
        /// Chamado pela: AuthBLL.Atualizar() → FormEditarAluno (tela de edição)
        /// </summary>
        public void Atualizar(Usuario usuario)
        {
            // "using" garante que a conexão será fechada ao sair do bloco, mesmo se der erro.
            using (var connection = ConnectionHelper.GetConnection())
            {
                // Comando SQL UPDATE: Altera os campos do registro WHERE Id = @Id.
                // Os valores com @ são parâmetros que serão substituídos de forma segura.
                string query = @"UPDATE Usuario 
                         SET Nome = @Nome, 
                             Telefone = @Telefone, 
                             Email = @Email, 
                             Cep = @Cep, 
                             Rua = @Rua, 
                             Bairro = @Bairro, 
                             Cidade = @Cidade, 
                             Estado = @Estado 
                         WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    // AddWithValue: Liga cada parâmetro @XXX ao valor real do objeto 'usuario'.
                    // Isso previne SQL Injection e trata tipos de dados automaticamente.
                    command.Parameters.AddWithValue("@Id", usuario.Id);
                    command.Parameters.AddWithValue("@Nome", usuario.Nome);
                    command.Parameters.AddWithValue("@Telefone", usuario.Telefone);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Cep", usuario.Cep);
                    command.Parameters.AddWithValue("@Rua", usuario.Rua);
                    command.Parameters.AddWithValue("@Bairro", usuario.Bairro);
                    command.Parameters.AddWithValue("@Cidade", usuario.Cidade);
                    command.Parameters.AddWithValue("@Estado", usuario.Estado);

                    // Abre a conexão com o banco de dados
                    connection.Open();

                    // ExecuteNonQuery: Executa o comando SQL sem retornar dados (INSERT, UPDATE, DELETE).
                    // Retorna a quantidade de linhas afetadas (neste caso, 1 se atualizou com sucesso).
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// EXCLUI um usuário do banco de dados pelo seu Id.
        /// 
        /// Como a tabela Pagamento e AcessoCatraca foram criadas com ON DELETE CASCADE
        /// no script SQL, ao excluir o usuário, todos os pagamentos e logs de catraca
        /// associados a ele são apagados AUTOMATICAMENTE pelo banco de dados.
        /// 
        /// Chamado pela: AuthBLL.ExcluirUsuario() → FormAdmin (botão "Excluir")
        /// </summary>
        public void Excluir(int id)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                // DELETE FROM: Remove o registro da tabela WHERE Id = @Id
                string query = "DELETE FROM Usuario WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// CADASTRA (insere) um novo usuário no banco de dados.
        /// 
        /// A senha já chega aqui como hash SHA256 (criptografada pela BLL).
        /// Esta método apenas insere os dados no banco, sem fazer validações.
        /// 
        /// Chamado pela: AuthBLL.Registrar() → FormRegistro (tela de cadastro)
        /// </summary>
        public void Cadastrar(Usuario usuario)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                // INSERT INTO: Adiciona uma nova linha na tabela Usuario.
                // O campo Id não é informado pois é IDENTITY (auto-incremento).
                // O campo DataCadastro também não, pois tem DEFAULT GETDATE() no banco.
                string query = @"INSERT INTO Usuario (Nome, Cpf, Telefone, Email, UsuarioLogin, SenhaHash, Cep, Rua, Bairro, Cidade, Estado, IsAdmin)
                                 VALUES (@Nome, @Cpf, @Telefone, @Email, @UsuarioLogin, @SenhaHash, @Cep, @Rua, @Bairro, @Cidade, @Estado, @IsAdmin)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nome", usuario.Nome);
                    command.Parameters.AddWithValue("@Cpf", usuario.Cpf);
                    command.Parameters.AddWithValue("@Telefone", usuario.Telefone);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@UsuarioLogin", usuario.UsuarioLogin);
                    command.Parameters.AddWithValue("@SenhaHash", usuario.SenhaHash);
                    command.Parameters.AddWithValue("@Cep", usuario.Cep);
                    command.Parameters.AddWithValue("@Rua", usuario.Rua);
                    command.Parameters.AddWithValue("@Bairro", usuario.Bairro);
                    command.Parameters.AddWithValue("@Cidade", usuario.Cidade);
                    command.Parameters.AddWithValue("@Estado", usuario.Estado);
                    command.Parameters.AddWithValue("@IsAdmin", usuario.IsAdmin);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// BUSCA um usuário pelo seu LOGIN (nome de usuário).
        /// 
        /// Usado no processo de autenticação (login): o sistema busca o usuário
        /// pelo login digitado e depois compara o hash da senha.
        /// Retorna null se o login não existir no banco.
        /// 
        /// Chamado pela: AuthBLL.Autenticar() → FormLogin (tela de login)
        /// </summary>
        public Usuario BuscarPorLogin(string login)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                // SELECT *: Traz todas as colunas do registro que possui o login informado.
                string query = "SELECT * FROM Usuario WHERE UsuarioLogin = @Login";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    connection.Open();

                    // ExecuteReader: Executa o SELECT e retorna um "leitor" (reader) de dados.
                    // O reader permite ler os dados linha por linha.
                    using (var reader = command.ExecuteReader())
                    {
                        // reader.Read(): Avança para a próxima linha do resultado.
                        // Se retornar true, significa que encontrou um registro.
                        if (reader.Read())
                        {
                            // Converte a linha do banco em um objeto Usuario usando o método auxiliar.
                            return MapReaderToUsuario(reader);
                        }
                    }
                }
            }
            // Se chegou aqui, não encontrou nenhum usuário com esse login.
            return null;
        }

        /// <summary>
        /// BUSCA um usuário pelo seu CPF.
        /// 
        /// Usado internamente pelo CatracaDAL para verificar pagamentos por CPF.
        /// Retorna null se o CPF não existir no banco.
        /// </summary>
        public Usuario BuscarPorCpf(string cpf)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                string query = "SELECT * FROM Usuario WHERE Cpf = @Cpf";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Cpf", cpf);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToUsuario(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// BUSCA um usuário pelo seu ID numérico.
        /// 
        /// Usado na tela administrativa para carregar os dados completos de um aluno
        /// antes de abrir a tela de edição (FormEditarAluno).
        /// Retorna null se o Id não existir no banco.
        /// 
        /// Chamado pela: AuthBLL.ObterPorId() → FormAdmin (botão "Editar")
        /// </summary>
        public Usuario BuscarPorId(int id)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                string query = "SELECT * FROM Usuario WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Reaproveita o método mapeador que já está pronto!
                            return MapReaderToUsuario(reader);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// MÉTODO AUXILIAR (privado) que converte uma linha do SqlDataReader em um objeto Usuario.
        /// 
        /// É um "mapeador" (mapper): pega cada coluna do resultado SQL e atribui à
        /// propriedade correspondente do objeto Usuario.
        /// 
        /// É privado (private) porque só é usado internamente por esta classe.
        /// Todos os métodos de busca (BuscarPorLogin, BuscarPorCpf, BuscarPorId)
        /// reutilizam este método, evitando repetição de código (princípio DRY).
        /// </summary>
        private Usuario MapReaderToUsuario(SqlDataReader reader)
        {
            return new Usuario
            {
                // reader["NomeColuna"]: Acessa o valor da coluna pelo nome.
                // Convert.ToInt32, Convert.ToBoolean, etc.: Converte o tipo genérico (object)
                // para o tipo correto do C#.
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"].ToString(),
                Cpf = reader["Cpf"].ToString(),
                Telefone = reader["Telefone"].ToString(),
                Email = reader["Email"].ToString(),
                UsuarioLogin = reader["UsuarioLogin"].ToString(),
                SenhaHash = reader["SenhaHash"].ToString(),
                Cep = reader["Cep"].ToString(),
                Rua = reader["Rua"].ToString(),
                Bairro = reader["Bairro"].ToString(),
                Cidade = reader["Cidade"].ToString(),
                Estado = reader["Estado"].ToString(),
                IsAdmin = Convert.ToBoolean(reader["IsAdmin"]),
                DataCadastro = Convert.ToDateTime(reader["DataCadastro"])
            };
        }


    }
}
