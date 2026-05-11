using SistemaAcademia.BLL.Validations;
using SistemaAcademia.DAL;
using SistemaAcademia.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SistemaAcademia.BLL.Auth
{
    public class AuthBLL
    {

        public void Registrar(Usuario usuario, string senha)
        {
            // 1. Aplicar as validações REGEX (Exigência do desafio)
            if (!RegexValidator.ValidarEmail(usuario.Email))
                throw new Exception("O formato do e-mail é inválido.");

            if (!RegexValidator.ValidarTelefone(usuario.Telefone))
                throw new Exception("O formato do telefone é inválido.");

            if (!RegexValidator.ValidarCep(usuario.Cep))
                throw new Exception("O formato do CEP é inválido.");

            // 2. Criptografa a senha
            usuario.SenhaHash = GerarHashSenha(senha);

            // 3. Manda para a DAL salvar no SQL Server usando o método Cadastrar
            UsuarioDAL dal = new UsuarioDAL();
            dal.Cadastrar(usuario);
        }

        public void Atualizar(Usuario usuario)
        {
            // 1. Aplicar as validações REGEX 
            if (!string.IsNullOrEmpty(usuario.Email) && !RegexValidator.ValidarEmail(usuario.Email))
                throw new Exception("O formato do e-mail é inválido.");

            if (!string.IsNullOrEmpty(usuario.Telefone) && !RegexValidator.ValidarTelefone(usuario.Telefone))
                throw new Exception("O formato do telefone é inválido.");

            if (!string.IsNullOrEmpty(usuario.Cep) && !RegexValidator.ValidarCep(usuario.Cep))
                throw new Exception("O formato do CEP é inválido.");

            // 2. Manda para a DAL atualizar no SQL Server
            UsuarioDAL dal = new UsuarioDAL();
            dal.Atualizar(usuario);
        }

        //Método para Excluir o usuário
        public void ExcluirUsuario(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            dal.Excluir(id);
        }

        // Método para buscar o usuário pelo ID (necessário para a tela de edição)
        public Usuario ObterPorId(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            return dal.BuscarPorId(id);
        }

        public Usuario Autenticar(string login, string senhaDigitada)
        {
            UsuarioDAL dal = new UsuarioDAL();

            // 1. Busca o usuário no banco de dados pelo Login
            Usuario usuario = dal.BuscarPorLogin(login);

            // 2. Verifica se o usuário existe
            if (usuario == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            // 3. Criptografa a senha que o cara digitou na tela de login para comparar
            string hashSenhaDigitada = GerarHashSenha(senhaDigitada);

            // 4. Compara os Hashes
            if (usuario.SenhaHash != hashSenhaDigitada)
            {
                throw new Exception("Senha incorreta.");
            }

            // Se passou por tudo, retorna o usuário logado (útil para saber se ele é Admin ou Cliente)
            return usuario;
        }

        // Método auxiliar para gerar o Hash da senha usando SHA256
        private string GerarHashSenha(string senha)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}