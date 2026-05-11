using System;
using SistemaAcademia.DAL;
using SistemaAcademia.Models;

namespace SistemaAcademia.BLL.Pagamento
{
    public class PagamentoBLL
    {
        private readonly PagamentoDAL _pagamentoDAL;

        public PagamentoBLL()
        {
            _pagamentoDAL = new PagamentoDAL();
        }

        public void RegistrarPagamentoMesVigente(int usuarioId, decimal valor)
        {
            PagamentoDAL dal = new PagamentoDAL();
            dal.InserirPagamento(usuarioId, valor);
        }
    }
}
