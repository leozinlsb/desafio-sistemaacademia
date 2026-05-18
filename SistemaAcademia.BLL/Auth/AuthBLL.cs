using SistemaAcademia.BLL.Validations;
using SistemaAcademia.DAL;
using SistemaAcademia.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SistemaAcademia.BLL.Auth
{
    /// <summary>
    /// BLL (Business Logic Layer / Camada de Regras de Negócio) de AUTENTICAÇÃO.
    /// 
    /// Esta é a classe central do sistema! Ela contém as REGRAS DE NEGÓCIO relacionadas
    /// a usuários: registro, autenticação (login), edição e exclusão.
    /// 
    /// Papel na arquitetura em 3 camadas:
    ///   UI (tela) → chama a BLL para processar
    ///   BLL (aqui) → valida dados, aplica regras, criptografa senhas
    ///   DAL (banco) → a BLL chama a DAL para ler/gravar no banco
    /// 
    /// A UI NUNCA acessa a DAL diretamente. Sempre passa pela BLL primeiro.
    /// </summary>
    public class AuthBLL
    {
        /// <summary>
        /// REGISTRA (cadastra) um novo usuário no sistema.
        /// 
        /// Fluxo completo:
        ///   1. Valida e-mail, telefone e CEP usando Regex (expressões regulares)
        ///   2. Criptografa a senha com SHA256 (nunca salvamos senha em texto puro!)
        ///   3. Envia para a DAL inserir no banco de dados SQL Server
        /// 
        /// Se qualquer validação falhar, lança uma Exception com mensagem amigável
        /// que é exibida na tela (FormRegistro) dentro de um MessageBox.
        /// 
        /// Chamado por: FormRegistro.btnCadastrar_Click()
        /// </summary>
        /// <param name="usuario">Objeto com os dados preenchidos na tela de registro</param>
        /// <param name="senha">Senha digitada pelo usuário (em texto puro, será criptografada)</param>
        public void Registrar(Usuario usuario, string senha)
        {
            // 1. Aplicar as validações REGEX (Exigência do desafio)
            // Se o formato não bater com o padrão esperado, lança exceção e para tudo.
            if (!RegexValidator.ValidarEmail(usuario.Email))
                throw new Exception("O formato do e-mail é inválido.");

            if (!RegexValidator.ValidarTelefone(usuario.Telefone))
                throw new Exception("O formato do telefone é inválido.");

            if (!RegexValidator.ValidarCep(usuario.Cep))
                throw new Exception("O formato do CEP é inválido.");

            // 2. Criptografa a senha digitada, transformando-a em um hash SHA256.
            // Exemplo: "admin123" → "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9"
            // É isso que será salvo no banco, nunca a senha real.
            usuario.SenhaHash = GerarHashSenha(senha);

            // 3. Manda para a DAL salvar no SQL Server usando o método Cadastrar
            UsuarioDAL dal = new UsuarioDAL();
            dal.Cadastrar(usuario);
        }

        /// <summary>
        /// ATUALIZA os dados de um usuário existente.
        /// 
        /// Valida os campos que foram preenchidos (ignora campos vazios na validação)
        /// e depois envia para a DAL atualizar no banco.
        /// 
        /// Chamado por: FormEditarAluno.btnSalvar_Click()
        /// </summary>
        public void Atualizar(Usuario usuario)
        {
            // 1. Validações Regex (só valida se o campo não está vazio)
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

        /// <summary>
        /// EXCLUI um usuário do sistema pelo Id.
        /// Chamado por: FormAdmin.btnExcluir_Click()
        /// </summary>
        public void ExcluirUsuario(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            dal.Excluir(id);
        }

        /// <summary>
        /// BUSCA um usuário pelo Id. Necessário para a tela de edição (FormEditarAluno).
        /// Chamado por: FormAdmin.btnEditar_Click()
        /// </summary>
        public Usuario ObterPorId(int id)
        {
            UsuarioDAL dal = new UsuarioDAL();
            return dal.BuscarPorId(id);
        }

        /// <summary>
        /// AUTENTICA (faz login) um usuário no sistema.
        /// 
        /// Fluxo completo:
        ///   1. Busca o usuário no banco pelo login digitado
        ///   2. Se não encontrou → lança exceção "Usuário não encontrado"
        ///   3. Criptografa a senha digitada na tela com SHA256
        ///   4. Compara o hash gerado com o hash salvo no banco
        ///   5. Se forem diferentes → lança exceção "Senha incorreta"
        ///   6. Se passou em tudo → retorna o objeto Usuario logado
        /// 
        /// O objeto retornado é usado pela tela para saber se é Admin ou Aluno:
        ///   - Admin (IsAdmin = true) → abre FormAdmin
        ///   - Aluno (IsAdmin = false) → verifica catraca
        /// 
        /// Chamado por: FormLogin.btnLogin_Click()
        /// </summary>
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

            // 3. Criptografa a senha digitada na tela para poder comparar
            // (no banco está salvo o hash, não a senha real)
            string hashSenhaDigitada = GerarHashSenha(senhaDigitada);

            // 4. Compara os Hashes: o do banco vs o que acabamos de gerar
            if (usuario.SenhaHash != hashSenhaDigitada)
            {
                throw new Exception("Senha incorreta.");
            }

            // Se passou por todas as verificações, retorna o usuário logado
            return usuario;
        }

        /// <summary>
        /// MÉTODO AUXILIAR PRIVADO: Gera o Hash SHA256 de uma senha.
        /// 
        /// SHA256 é um algoritmo de hash criptográfico que transforma qualquer texto
        /// em uma sequência fixa de 64 caracteres hexadecimais (256 bits).
        /// 
        /// Características importantes:
        ///   - É unidirecional: não dá para "descriptografar" o hash de volta para a senha
        ///   - A mesma entrada sempre gera a mesma saída (determinístico)
        ///   - Qualquer mudança mínima na entrada gera um hash completamente diferente
        /// 
        /// Exemplo: "admin123" → "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9"
        /// </summary>
        private string GerarHashSenha(string senha)
        {
            // SHA256.Create(): Cria uma instância do algoritmo SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                // 1. Converte a string da senha em bytes (array de bytes) usando UTF8
                // 2. ComputeHash: Aplica o algoritmo SHA256 nesses bytes, gerando o hash
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));

                // 3. Converte o array de bytes do hash para uma string hexadecimal legível
                // Cada byte vira 2 caracteres hex (ex: 255 → "ff"). "x2" = formato hex minúsculo.
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