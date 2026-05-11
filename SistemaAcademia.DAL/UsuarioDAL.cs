using System;
using Microsoft.Data.SqlClient;
using SistemaAcademia.Models;

namespace SistemaAcademia.DAL
{
    public class UsuarioDAL
    {
        public void Atualizar(Usuario usuario)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                // Atualiza apenas os dados permitidos (nunca atualizamos o CPF ou o Login para manter a integridade)
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
                    command.Parameters.AddWithValue("@Id", usuario.Id);
                    command.Parameters.AddWithValue("@Nome", usuario.Nome);
                    command.Parameters.AddWithValue("@Telefone", usuario.Telefone);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@Cep", usuario.Cep);
                    command.Parameters.AddWithValue("@Rua", usuario.Rua);
                    command.Parameters.AddWithValue("@Bairro", usuario.Bairro);
                    command.Parameters.AddWithValue("@Cidade", usuario.Cidade);
                    command.Parameters.AddWithValue("@Estado", usuario.Estado);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Excluir(int id)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                // Deleta o usuário pelo Id. 
                // Como você usou 'ON DELETE CASCADE' na tabela de pagamentos no script DDL, 
                // os pagamentos desse aluno serão apagados automaticamente!
                string query = "DELETE FROM Usuario WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Cadastrar(Usuario usuario)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
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

        public Usuario BuscarPorLogin(string login)
        {
            using (var connection = ConnectionHelper.GetConnection())
            {
                string query = "SELECT * FROM Usuario WHERE UsuarioLogin = @Login";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);
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
                            // Reaproveita o seu método mapeador que já está pronto!
                            return MapReaderToUsuario(reader);
                        }
                    }
                }
            }
            return null;
        }

        private Usuario MapReaderToUsuario(SqlDataReader reader)
        {
            return new Usuario
            {
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
