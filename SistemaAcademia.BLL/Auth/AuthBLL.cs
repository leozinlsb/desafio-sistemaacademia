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
            if (!RegexValidator.ValidarEmail(usuario.Email))
                throw new Exception("O formato do e-mail é inválido.");

            if (!RegexValidator.ValidarTelefone(usuario.Telefone))
                throw new Exception("O formato do telefone é inválido.");

            if (!RegexValidator.ValidarCep(usuario.Cep))
                throw new Exception("O formato do CEP é inválido.");

            usuario.SenhaHash = GerarHashSenha(senha);

            UsuarioDAL dal = new UsuarioDAL();
            dal.Cadastrar(usuario);
        }

        public void Atualizar(Usuario usuario)
        {
            if (!string.IsNullOrEmpty(usuario.Email) && !RegexValidator.ValidarEmail(usuario.Email))
                throw new Exception("O formato do e-mail é inválido.");

            if (!string.IsNullOrEmpty(usuario.Telefone) && !RegexValidator.ValidarTelefone(usuario.Telefone))
                throw new Exception("O formato do telefone é inválido.");

            if (!string.IsNullOrEmpty(usuario.Cep) && !RegexValidator.ValidarCep(usuario.Cep))
                throw new Exception("O formato do CEP é inválido.");

            UsuarioDAL dal = new UsuarioDAL();
            dal.Atualizar(usuario);
        }

        public void ExcluirUsuario(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            dal.Excluir(id);
        }

        public Usuario ObterPorId(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            return dal.BuscarPorId(id);
        }

        public Usuario Autenticar(string login, string senhaDigitada)
        {
            UsuarioDAL dal = new UsuarioDAL();
            Usuario usuario = dal.BuscarPorLogin(login);

            if (usuario == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            string hashSenhaDigitada = GerarHashSenha(senhaDigitada);

            if (usuario.SenhaHash != hashSenhaDigitada)
            {
                throw new Exception("Senha incorreta.");
            }

            return usuario;
        }

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