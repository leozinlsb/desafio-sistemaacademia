using System;

namespace SistemaAcademia.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string UsuarioLogin { get; set; }
        public string SenhaHash { get; set; }
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
