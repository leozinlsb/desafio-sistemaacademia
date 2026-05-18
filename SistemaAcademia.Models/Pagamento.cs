using System;

namespace SistemaAcademia.Models
{
    /// <summary>
    /// MODELO (Model) que representa um PAGAMENTO de mensalidade da academia.
    /// 
    /// Esta classe é um "espelho" da tabela "Pagamento" do banco de dados.
    /// Cada registro representa um pagamento feito por um aluno para um mês específico.
    /// 
    /// Fluxo: O administrador seleciona um aluno na tela de admin e clica em
    /// "Registrar Pagamento". Isso cria um novo registro na tabela Pagamento
    /// para o mês/ano atuais. Depois disso, quando o aluno tentar passar na catraca,
    /// o sistema encontra esse pagamento e libera a entrada.
    /// </summary>
    public class Pagamento
    {
        // Id: Identificador único do pagamento (chave primária, auto-incremento)
        public int Id { get; set; }

        // UsuarioId: Id do aluno que fez o pagamento (chave estrangeira → tabela Usuario)
        public int UsuarioId { get; set; }

        // Valor: Valor pago em reais (ex: 100.00). O tipo decimal é ideal para valores monetários
        // pois evita problemas de arredondamento que o float/double teriam.
        public decimal Valor { get; set; }

        // DataPagamento: Data e hora em que o pagamento foi registrado no sistema
        public DateTime DataPagamento { get; set; }

        // MesReferencia: Mês a que este pagamento se refere (1 = Janeiro, 12 = Dezembro)
        // Ex: Se MesReferencia = 5, o pagamento é referente ao mês de Maio.
        public int MesReferencia { get; set; }

        // AnoReferencia: Ano a que este pagamento se refere (ex: 2026)
        // Junto com MesReferencia, forma o par que identifica O PERÍODO do pagamento.
        // O sistema verifica se existe um pagamento para o mês/ano ATUAL para liberar a catraca.
        public int AnoReferencia { get; set; }
    }
}
