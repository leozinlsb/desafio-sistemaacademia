using System;

namespace SistemaAcademia.Models
{
    /// <summary>
    /// MODELO (Model) que representa um REGISTRO DE ACESSO À CATRACA.
    /// 
    /// Esta classe é um "espelho" da tabela "AcessoCatraca" do banco de dados.
    /// Cada vez que um aluno tenta passar na catraca (seja pela tela de login
    /// ou pela simulação do admin), um registro deste tipo é salvo no banco,
    /// criando um HISTÓRICO (log) de todas as tentativas de entrada.
    /// </summary>
    public class AcessoCatraca
    {
        // Id: Identificador único do registro de acesso (chave primária, auto-incremento)
        public int Id { get; set; }

        // UsuarioId: Id do aluno que tentou acessar a academia (chave estrangeira → tabela Usuario)
        public int UsuarioId { get; set; }

        // DataAcesso: Data e hora exata em que a tentativa de passagem pela catraca aconteceu
        public DateTime DataAcesso { get; set; }

        // AcessoLiberado: Indica se a catraca foi liberada (true) ou bloqueada (false).
        // true = aluno tinha pagamento em dia → catraca liberou a passagem
        // false = aluno estava inadimplente → catraca bloqueou a entrada
        public bool AcessoLiberado { get; set; }

        // MotivoBloqueio: Texto explicando por que o acesso foi negado (ex: "Inadimplência").
        // Pode ser null quando o acesso foi liberado (não houve bloqueio).
        public string MotivoBloqueio { get; set; }
    }
}
