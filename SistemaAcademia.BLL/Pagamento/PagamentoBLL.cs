using System;
using SistemaAcademia.DAL;
using SistemaAcademia.Models;

namespace SistemaAcademia.BLL.Pagamento
{
    /// <summary>
    /// BLL (Business Logic Layer) de PAGAMENTOS.
    /// 
    /// Contém as regras de negócio relacionadas a pagamentos de mensalidade.
    /// Atualmente, o método principal registra o pagamento do mês vigente (atual)
    /// para um aluno específico.
    /// 
    /// Chamada por: FormAdmin.btnRegistrarPagamento_Click()
    /// </summary>
    public class PagamentoBLL
    {
        // Referência à DAL de pagamentos (instanciada no construtor)
        private readonly PagamentoDAL _pagamentoDAL;

        /// <summary>
        /// Construtor: Cria a instância da PagamentoDAL.
        /// </summary>
        public PagamentoBLL()
        {
            _pagamentoDAL = new PagamentoDAL();
        }

        /// <summary>
        /// REGISTRA O PAGAMENTO DO MÊS ATUAL para um aluno.
        /// 
        /// O admin seleciona um aluno na grid e clica em "Registrar Pagamento".
        /// Este método recebe o Id do aluno e o valor (fixo em R$ 100,00 na tela)
        /// e envia para a DAL inserir no banco.
        /// 
        /// Após esse registro, quando o aluno tentar passar na catraca, o sistema
        /// encontrará este pagamento e liberará a entrada.
        /// 
        /// Chamado por: FormAdmin.btnRegistrarPagamento_Click()
        /// </summary>
        /// <param name="usuarioId">Id do aluno que está pagando</param>
        /// <param name="valor">Valor do pagamento (ex: 100.00)</param>
        public void RegistrarPagamentoMesVigente(int usuarioId, decimal valor)
        {
            PagamentoDAL dal = new PagamentoDAL();
            // A DAL usa GETDATE() do SQL para preencher data, mês e ano automaticamente
            dal.InserirPagamento(usuarioId, valor);
        }
    }
}
