using System;

namespace SistemaAcademia.Models
{
    /// <summary>
    /// MODELO (Model) que representa um USUÁRIO do sistema.
    /// 
    /// Esta classe é um "espelho" da tabela "Usuario" do banco de dados SQL Server.
    /// Cada propriedade aqui corresponde a uma coluna da tabela.
    /// 
    /// Ela é usada para transportar os dados entre as camadas do sistema:
    ///   UI (tela) → BLL (regras) → DAL (banco de dados) e vice-versa.
    /// 
    /// Um usuário pode ser ALUNO (IsAdmin = false) ou ADMINISTRADOR (IsAdmin = true).
    /// </summary>
    public class Usuario
    {
        // Id: Identificador único do usuário no banco (chave primária, auto-incremento)
        public int Id { get; set; }

        // Nome: Nome completo do aluno ou administrador
        public string Nome { get; set; }

        // Cpf: CPF com máscara (ex: "123.456.789-00") – é único no banco
        public string Cpf { get; set; }

        // Telefone: Telefone com máscara (ex: "(11) 99999-9999")
        public string Telefone { get; set; }

        // Email: E-mail do usuário – também é único no banco
        public string Email { get; set; }

        // UsuarioLogin: Nome de login que o usuário digita para entrar no sistema (ex: "joao123")
        public string UsuarioLogin { get; set; }

        // SenhaHash: A senha NÃO é armazenada em texto puro. Aqui fica o hash SHA256 da senha.
        // Exemplo: a senha "admin123" fica como "240be518fabd2724..." (64 caracteres hexadecimais)
        public string SenhaHash { get; set; }

        // Cep: CEP do endereço com máscara (ex: "01001-000")
        public string Cep { get; set; }

        // Rua: Logradouro do endereço (preenchido automaticamente pela API ViaCEP)
        public string Rua { get; set; }

        // Bairro: Bairro do endereço (preenchido automaticamente pela API ViaCEP)
        public string Bairro { get; set; }

        // Cidade: Cidade do endereço (preenchido automaticamente pela API ViaCEP)
        public string Cidade { get; set; }

        // Estado: Sigla do estado, ex: "SP", "RJ" (preenchido automaticamente pela API ViaCEP)
        public string Estado { get; set; }

        // IsAdmin: Define se o usuário é administrador (true) ou aluno comum (false).
        // Quando true, ao fazer login ele é redirecionado para a tela de administração.
        // Quando false, ao fazer login a catraca é verificada (liberada ou bloqueada).
        public bool IsAdmin { get; set; }

        // DataCadastro: Data e hora em que o usuário foi cadastrado no sistema.
        // Preenchido automaticamente pelo banco (GETDATE()) na hora da inserção.
        public DateTime DataCadastro { get; set; }
    }
}
